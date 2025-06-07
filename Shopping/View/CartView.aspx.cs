using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;

namespace Shopping.View
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCartItems();
            }
        }

        private void LoadCartItems()
        {
            if (Session["EmployeeID"] == null)
            {
                // Check if this is an async postback. If so, redirect client-side.
                if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", "window.location.href = '" + ResolveUrl("~/View/Login.aspx") + "';", true);
                }
                else
                {
                    Response.Redirect("~/View/Login.aspx");
                }
                return;
            }

            int employeeId = Convert.ToInt32(Session["EmployeeID"]); 
            CartController cartController = new CartController();
            List<CartItem> cartItems = cartController.GetCartItems(employeeId);

            rptShoppingCart.DataSource = cartItems;
            rptShoppingCart.DataBind();

            decimal totalPrice = 0;
            int totalItems = 0;

            foreach (var item in cartItems)
            {
                totalPrice += item.Quantity * item.UnitPrice;
                totalItems += item.Quantity;
            }

            litItemCount.Text = totalItems.ToString();
            litSummaryItemCount.Text = totalItems.ToString();
            litSummaryTotalPrice.Text = totalPrice.ToString("N0") + "$"; // Format as currency
            litTotalAmount.Text = totalPrice.ToString("N0") + "$"; // Format as currency

            // Update the cart count on the master page
            SiteMaster masterPage = this.Master as SiteMaster;
            if (masterPage != null)
            {
                masterPage.UpdateCartCount();
            }
        }

        protected void rptShoppingCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                // If session is null during an async postback, redirect client-side.
                if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", "window.location.href = '" + ResolveUrl("~/View/Login.aspx") + "';", true);
                }
                else
                {
                    Response.Redirect("~/View/Login.aspx");
                }
                return;
            }

            int employeeId = Convert.ToInt32(Session["EmployeeID"]); 
            CartController cartController = new CartController();
            int productId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Increase" || e.CommandName == "Decrease")
            {
                // Find the TextBox within the RepeaterItem
                TextBox txtQuantity = e.Item.FindControl("txtQuantity") as TextBox;
                if (txtQuantity != null)
                {
                    int currentQuantity = 0;
                    // Try to parse the current quantity, default to 1 if parsing fails
                    if (!int.TryParse(txtQuantity.Text, out currentQuantity))
                    {
                        currentQuantity = 1;
                    }
                    
                    int newQuantity = currentQuantity;

                    // Get the current list of cart items to find the UnitsInStock
                    List<CartItem> currentCartItems = cartController.GetCartItems(employeeId);
                    CartItem currentProduct = currentCartItems.FirstOrDefault(item => item.ProductID == productId);

                    if (currentProduct != null)
                    {
                        int maxStock = currentProduct.UnitsInStock;

                        if (e.CommandName == "Increase")
                        {
                            newQuantity++;
                            if (newQuantity > maxStock)
                            {
                                newQuantity = maxStock; // Limit to available stock
                                // Optionally, you could display a message to the user here.
                                // For server-side, this usually means reloading the cart with the adjusted quantity
                            }
                        }
                        else if (e.CommandName == "Decrease")
                        {
                            newQuantity--;
                        }

                        // Ensure quantity doesn't go below 1
                        if (newQuantity < 1) newQuantity = 1;

                        cartController.UpdateCartItemQuantity(employeeId, productId, newQuantity);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error: Product not found in cart for ProductID: {productId}");
                    }
                }
                else
                {
                    // Log or handle the case where txtQuantity is not found
                    System.Diagnostics.Debug.WriteLine($"Error: txtQuantity control not found for ProductID: {productId}");
                }
            }
            else if (e.CommandName == "Remove")
            {
                cartController.RemoveCartItem(employeeId, productId);
            }

            // Re-load cart items to refresh the display
            LoadCartItems();

            // Update the cart count on the master page
            SiteMaster masterPage = this.Master as SiteMaster;
            if (masterPage != null)
            {
                masterPage.UpdateCartCount();
            }
        }
    }
}