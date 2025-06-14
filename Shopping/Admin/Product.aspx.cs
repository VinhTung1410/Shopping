using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        private ProductController productController;

        protected void Page_Load(object sender, EventArgs e)
        {
            productController = new ProductController();

            if (!IsPostBack)
            {
                BindGrid();
                ViewState["IsEdit"] = false;
            }
        }

        private void BindGrid()
        {
            var products = productController.GetAllProducts();
            gvProducts.DataSource = products;
            gvProducts.DataBind();
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var product = e.Row.DataItem as Model1.Product;
                if (product != null && product.ProductImages != null)
                {
                    var rptThumbnails = (Repeater)e.Row.FindControl("rptThumbnails");
                    if (rptThumbnails != null)
                    {
                        rptThumbnails.DataSource = product.ProductImages;
                        rptThumbnails.DataBind();
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            pnlAddEdit.Visible = true;
            //txtProductID.Enabled = true;
            ViewState["IsEdit"] = false;
            ViewState["EditProductID"] = null;
            rptProductImages.DataSource = null;
            rptProductImages.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    bool isEdit = (bool)ViewState["IsEdit"];
                    int? productId = isEdit ? (int?)ViewState["EditProductID"] : null;

                    var product = new Model1.Product
                    {
                        ProductID = productId ?? 0,
                        ProductName = txtProductName.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        UnitPrice = decimal.Parse(txtUnitPrice.Text),
                        UnitsInStock = int.Parse(txtUnitsInStock.Text)
                    };

                    bool success;
                    if (isEdit)
                    {
                        success = productController.UpdateProduct(product);
                        if (success)
                        {
                            HandleImageUploads(product.ProductID);
                            ShowMessage(true, "Product has been updated!", null);
                        }
                        else
                        {
                            ShowMessage(false, null, "Error updating product.");
                        }
                    }
                    else
                    {
                        success = productController.AddProduct(product);
                        if (success)
                        {
                            // Get the new product ID after insert
                            var newProduct = productController.GetAllProducts()
                                .FirstOrDefault(p => p.ProductName == product.ProductName);
                            if (newProduct != null)
                            {
                                HandleImageUploads(newProduct.ProductID);
                            }
                            ShowMessage(true, "Product has been added!", null);
                        }
                        else
                        {
                            ShowMessage(false, null, "Error adding product.");
                        }
                    }

                    if (success)
                    {
                        pnlAddEdit.Visible = false;
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(false, null, "Error: " + ex.Message);
            }
        }

        private void HandleImageUploads(int productId)
        {
            if (fuProductImages.HasFiles)
            {
                List<string> errorMessages = new List<string>();
                bool hasSuccess = false;

                foreach (HttpPostedFile file in fuProductImages.PostedFiles)
                {
                    try
                    {
                        var validationResult = ImageHelper.ValidateImage(file);
                        if (validationResult.IsValid)
                        {
                            if (productController.AddProductImage(productId, file))
                            {
                                hasSuccess = true;
                            }
                            else
                            {
                                errorMessages.Add($"Failed to save image: {file.FileName}");
                            }
                        }
                        else
                        {
                            errorMessages.Add($"Invalid image {file.FileName}: {validationResult.ErrorMessage}");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add($"Error processing {file.FileName}: {ex.Message}");
                    }
                }

                if (errorMessages.Any())
                {
                    lblImageError.Text = string.Join("<br/>", errorMessages);
                    lblImageError.Visible = true;
                }

                if (hasSuccess)
                {
                    // Refresh the product images
                    var product = productController.GetProductById(productId);
                    rptProductImages.DataSource = product.ProductImages;
                    rptProductImages.DataBind();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlAddEdit.Visible = false;
            ClearForm();
            ViewState["IsEdit"] = false;
            ViewState["EditProductID"] = null;
            lblImageError.Visible = false;
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditProduct":
                        Model1.Product product = productController.GetProductById(productId);
                        if (product != null)
                        {
                            ViewState["IsEdit"] = true;
                            ViewState["EditProductID"] = productId;
                            pnlAddEdit.Visible = true;
                            //txtProductID.Text = product.ProductID.ToString();
                            //txtProductID.Enabled = false;
                            txtProductName.Text = product.ProductName;
                            txtDescription.Text = product.Description;
                            //txtQuantityPerUnit.Text = product.QuantityPerUnit;
                            txtUnitPrice.Text = product.UnitPrice?.ToString();
                            txtUnitsInStock.Text = product.UnitsInStock?.ToString();
                            //txtUnitsOnOrder.Text = product.UnitsOnOrder?.ToString();

                            // Bind product images
                            if (product.ProductImages != null)
                            {
                                rptProductImages.DataSource = product.ProductImages;
                                rptProductImages.DataBind();
                            }
                        }
                        break;

                    case "DeleteProduct":
                        bool success = productController.DeleteProduct(productId);
                        ShowMessage(success, "Product has been deleted!", "Error deleting product.");
                        if (success)
                        {
                            BindGrid();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(false, null, "Error: " + ex.Message);
            }
        }

        protected void rptProductImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteImage")
            {
                int imageId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    bool success = productController.DeleteProductImage(imageId);
                    if (success)
                    {
                        // Refresh the product images
                        int productId = (int)ViewState["EditProductID"];
                        var product = productController.GetProductById(productId);
                        rptProductImages.DataSource = product.ProductImages;
                        rptProductImages.DataBind();
                        ShowMessage(true, "Image has been removed!", null);
                    }
                    else
                    {
                        ShowMessage(false, null, "Error deleting image.");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(false, null, "Error deleting image: " + ex.Message);
                }
            }
        }

        private void ClearForm()
        {
            //txtProductID.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtUnitsInStock.Text = string.Empty;
            lblImageError.Visible = false;
            rptProductImages.DataSource = null;
            rptProductImages.DataBind();
        }

        private void ShowMessage(bool success, string successMessage, string errorMessage)
        {
            litMessage.Text = success ? successMessage : errorMessage;
            pnlMessage.Visible = true;
            pnlMessage.CssClass = success ? "alert alert-success" : "alert alert-danger";

            ScriptManager.RegisterStartupScript(this, GetType(), "HideMessage",
                "setTimeout(function() { document.getElementById('" + pnlMessage.ClientID + "').style.display = 'none'; }, 3000);",
                true);
        }
    }
}