using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.View
{
    public partial class Product : System.Web.UI.Page
    {
        private ProductController productController;
        private bool isEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            productController = new ProductController();

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            gvProducts.DataSource = productController.GetAllProducts();
            gvProducts.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            pnlAddEdit.Visible = true;
            txtProductID.Enabled = true;
            isEdit = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    var product = new Model1.Product
                    {
                        ProductID = Convert.ToInt32(txtProductID.Text),
                        ProductName = txtProductName.Text,
                        QuantityPerUnit = string.IsNullOrEmpty(txtQuantityPerUnit.Text) ? null : txtQuantityPerUnit.Text,
                        UnitPrice = string.IsNullOrEmpty(txtUnitPrice.Text) ? null : (decimal?)Convert.ToDecimal(txtUnitPrice.Text),
                        UnitsInStock = string.IsNullOrEmpty(txtUnitsInStock.Text) ? null : (int?)Convert.ToInt32(txtUnitsInStock.Text),
                        UnitsOnOrder = string.IsNullOrEmpty(txtUnitsOnOrder.Text) ? null : (int?)Convert.ToInt32(txtUnitsOnOrder.Text)
                    };

                    bool success;
                    if (isEdit)
                    {
                        success = productController.UpdateProduct(product);
                        ShowMessage(success, "Product updated successfully!", "Error updating product.");
                    }
                    else
                    {
                        success = productController.AddProduct(product);
                        ShowMessage(success, "Product added successfully!", "Error adding product.");
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
                ShowMessage(false, "", "Error: " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlAddEdit.Visible = false;
            ClearForm();
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
                            isEdit = true;
                            pnlAddEdit.Visible = true;
                            txtProductID.Text = product.ProductID.ToString();
                            txtProductID.Enabled = false;
                            txtProductName.Text = product.ProductName;
                            txtQuantityPerUnit.Text = product.QuantityPerUnit;
                            txtUnitPrice.Text = product.UnitPrice?.ToString();
                            txtUnitsInStock.Text = product.UnitsInStock?.ToString();
                            txtUnitsOnOrder.Text = product.UnitsOnOrder?.ToString();
                        }
                        break;

                    case "DeleteProduct":
                        bool success = productController.DeleteProduct(productId);
                        ShowMessage(success, "Product deleted successfully!", "Error deleting product.");
                        if (success)
                        {
                            BindGrid();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(false, "", "Error: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtProductID.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtQuantityPerUnit.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtUnitsInStock.Text = string.Empty;
            txtUnitsOnOrder.Text = string.Empty;
        }

        private void ShowMessage(bool success, string successMessage, string errorMessage)
        {
            litMessage.Text = success ? successMessage : errorMessage;
            pnlMessage.Visible = true;
            pnlMessage.CssClass = success ? "alert alert-success" : "alert alert-danger";

            // Automatically hide message after 3 seconds
            ScriptManager.RegisterStartupScript(this, GetType(), "HideMessage",
                "setTimeout(function() { document.getElementById('" + pnlMessage.ClientID + "').style.display = 'none'; }, 3000);",
                true);
        }
    }
}