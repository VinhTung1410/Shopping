using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;

namespace Shopping
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmployeeID"] != null)
            {
                int employeeId = Convert.ToInt32(Session["EmployeeID"]);
                CartController cartController = new CartController();
                List<CartItem> cartItems = cartController.GetCartItems(employeeId);
                
                int totalItemsInCart = cartItems.Sum(item => item.Quantity);
                lblCartItemCount.Text = totalItemsInCart.ToString();
                cartPanel.Visible = true;
            }
            else
            {
                cartPanel.Visible = false;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session variables
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("~/View/Login.aspx");
        }

        public void UpdateCartCount()
        {
            if (Session["EmployeeID"] != null)
            {
                int employeeId = Convert.ToInt32(Session["EmployeeID"]);
                CartController cartController = new CartController();
                var cartItems = cartController.GetCartItems(employeeId);
                lblCartItemCount.Text = cartItems.Sum(item => item.Quantity).ToString();
                updCartCount.Update();
            }
            else
            {
                lblCartItemCount.Text = "0";
            }
        }
    }
}