using System;
using System.Web.UI.WebControls;
using Shopping.Controller1;
using Shopping.Model1;
using System.Collections.Generic;

namespace Shopping.View
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        private ProductDetailsController controller = new ProductDetailsController();
        private int ProductId
        {
            get
            {
                int id;
                int.TryParse(Request.QueryString["productId"], out id);
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProductDetails();
            }
        }

        private void LoadProductDetails()
        {
            if (ProductId <= 0)
            {
                lblProductName.Text = "Không tìm thấy sản phẩm";
                return;
            }
            var product = controller.GetProductDetails(ProductId);
            if (product == null)
            {
                lblProductName.Text = "Không tìm thấy sản phẩm";
                return;
            }
            lblProductName.Text = product.ProductName;
            lblDescription.Text = product.Description;
            lblUnitPrice.Text = product.UnitPrice.HasValue ? product.UnitPrice.Value.ToString("N0") : "";
            lblUnitsInStock.Text = product.UnitsInStock.HasValue ? product.UnitsInStock.Value.ToString() : "";

            // Bind images
            ImagesRepeater.DataSource = product.ProductImages;
            ImagesRepeater.DataBind();

            txtQuantity.Text = "1";
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            int qty = 1;
            int.TryParse(txtQuantity.Text, out qty);
            if (qty > 1) qty--;
            txtQuantity.Text = qty.ToString();
        }

        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            int qty = 1;
            int.TryParse(txtQuantity.Text, out qty);
            qty++;
            txtQuantity.Text = qty.ToString();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                Response.Redirect("~/View/Login.aspx");
                return;
            }

            int qty = 1;
            int.TryParse(txtQuantity.Text, out qty);
            int maxStock = 0;
            int.TryParse(lblUnitsInStock.Text, out maxStock);
            if (qty > maxStock)
            {
                // Display error message to the user
                // You might want to use a Label control on the ASPX page for this
                // Example: lblErrorMessage.Text = "Số lượng vượt quá tồn kho!";
                return;
            }

            // Assuming a UserId of 1 for now, replace with actual logged-in user's ID
            int employeeId = Convert.ToInt32(Session["EmployeeID"]); 
            CartController cartController = new CartController();
            cartController.AddToCart(employeeId, ProductId, qty);

            Response.Redirect("CartView.aspx");
        }

        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                Response.Redirect("~/View/Login.aspx");
                return;
            }

            // For "Buy Now", we can reuse the AddToCart logic and then immediately redirect to CartView.aspx
            // This assumes "Buy Now" means adding to cart and proceeding to checkout.
            // If "Buy Now" has a different flow (e.g., direct checkout), this logic needs adjustment.
            
            // Get quantity from the textbox
            int qty = 1;
            int.TryParse(txtQuantity.Text, out qty);
            int maxStock = 0;
            int.TryParse(lblUnitsInStock.Text, out maxStock);
            if (qty > maxStock)
            {
                // Display error message to the user
                return;
            }

            int employeeId = Convert.ToInt32(Session["EmployeeID"]); 
            CartController cartController = new CartController();
            cartController.AddToCart(employeeId, ProductId, qty);

            Response.Redirect("CartView.aspx");
        }
    }
}