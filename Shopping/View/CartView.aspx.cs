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
        private CartController cartController;

        protected void Page_Load(object sender, EventArgs e)
        {
            cartController = new CartController();
            
            if (!IsPostBack)
            {
                LoadCartItems();
            }
        }

        private void LoadCartItems(List<CartItem> cartItems = null)
        {
            if (Session["EmployeeID"] == null)
            {
                Response.Redirect("~/View/Login.aspx");
                return;
            }

            try
            {
                int employeeId = Convert.ToInt32(Session["EmployeeID"]); 
                
                // If cartItems is not provided, get them from the controller
                if (cartItems == null)
                {
                    cartItems = cartController.GetCartItems(employeeId);
                }

                if (cartItems != null)
                {
                    rptShoppingCart.DataSource = cartItems;
                    rptShoppingCart.DataBind();

                    decimal totalPrice = 0;
                    int totalItems = 0;

                    foreach (var item in cartItems)
                    {
                        totalPrice += item.Quantity * item.UnitPrice;
                        totalItems += item.Quantity;
                    }

                    // Store base price in Session for shipping calculations
                    Session["BasePrice"] = totalPrice;

                    if (litItemCount != null) litItemCount.Text = totalItems.ToString();
                    if (litSummaryItemCount != null) litSummaryItemCount.Text = totalItems.ToString();
                    if (litSummaryTotalPrice != null) litSummaryTotalPrice.Text = totalPrice.ToString("N0") + "€";
                    if (lblTotalAmount != null) lblTotalAmount.Text = totalPrice.ToString("N0") + "€";
                    if (lblFinalTotal != null) lblFinalTotal.Text = totalPrice.ToString("N0") + "€";

                    // Update the cart count on the master page
                    SiteMaster masterPage = this.Master as SiteMaster;
                    if (masterPage != null)
                    {
                        masterPage.UpdateCartCount();
                    }
                }
            }
            catch (Exception ex)
            {
                if (lblMessage != null)
                {
                    lblMessage.Text = "Error loading cart items: " + ex.Message;
                    lblMessage.Visible = true;
                }
                System.Diagnostics.Debug.WriteLine($"Error in LoadCartItems: {ex.Message}");
            }
        }

        protected void rptShoppingCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                Response.Redirect("~/View/Login.aspx");
                return;
            }

            try
            {
                int employeeId = Convert.ToInt32(Session["EmployeeID"]); 
                int productId = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Increase" || e.CommandName == "Decrease")
                {
                    TextBox txtQuantity = e.Item.FindControl("txtQuantity") as TextBox;
                    if (txtQuantity != null)
                    {
                        int currentQuantity = 0;
                        if (!int.TryParse(txtQuantity.Text, out currentQuantity))
                        {
                            currentQuantity = 1;
                        }
                        
                        int newQuantity = currentQuantity;
                        List<CartItem> currentCartItems = cartController.GetCartItems(employeeId);
                        CartItem currentProduct = currentCartItems?.FirstOrDefault(item => item.ProductID == productId);

                        if (currentProduct != null)
                        {
                            int maxStock = currentProduct.UnitsInStock;

                            if (e.CommandName == "Increase")
                            {
                                newQuantity++;
                                if (newQuantity > maxStock)
                                {
                                    newQuantity = maxStock;
                                }
                            }
                            else if (e.CommandName == "Decrease")
                            {
                                newQuantity--;
                            }

                            if (newQuantity < 1) newQuantity = 1;

                            cartController.UpdateCartItemQuantity(employeeId, productId, newQuantity);
                        }
                    }
                }
                else if (e.CommandName == "Remove")
                {
                    cartController.RemoveCartItem(employeeId, productId);
                }

                LoadCartItems();
            }
            catch (Exception ex)
            {
                if (lblMessage != null)
                {
                    lblMessage.Text = "Error processing cart item: " + ex.Message;
                    lblMessage.Visible = true;
                }
                System.Diagnostics.Debug.WriteLine($"Error in rptShoppingCart_ItemCommand: {ex.Message}");
            }
        }

        // Add method to handle shipping cost updates
        [System.Web.Services.WebMethod]
        public static string UpdateTotalWithShipping(decimal shippingCost)
        {
            try
            {
                // Get the base price from the session
                decimal basePrice = 0;
                if (HttpContext.Current.Session["BasePrice"] != null)
                {
                    basePrice = Convert.ToDecimal(HttpContext.Current.Session["BasePrice"]);
                }

                // Calculate new total
                decimal newTotal = basePrice + shippingCost;

                // Store the new total in session
                HttpContext.Current.Session["FinalTotal"] = newTotal;

                // Return formatted total
                return newTotal.ToString("N0") + "€";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["Cart"] != null)
            {
                List<CartItem> cartItems = (List<CartItem>)Session["Cart"];
                LoadCartItems(cartItems);
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                Response.Redirect("~/View/Login.aspx");
                return;
            }

            try
            {
                int employeeId = Convert.ToInt32(Session["EmployeeID"]);
                var cartItems = cartController.GetCartItems(employeeId);
                
                if (cartItems == null || !cartItems.Any())
                {
                    if (lblMessage != null)
                    {
                        lblMessage.Text = "Your cart is empty.";
                        lblMessage.Visible = true;
                    }
                    return;
                }

                foreach (var item in cartItems)
                {
                    if (item.Quantity > item.UnitsInStock)
                    {
                        if (lblMessage != null)
                        {
                            lblMessage.Text = $"Insufficient stock for product: {item.ProductName}. Available: {item.UnitsInStock}";
                            lblMessage.Visible = true;
                        }
                        return;
                    }
                }

                cartController.CompleteOrder(employeeId);
                Response.Redirect("~/View/OrderSuccess.aspx");
            }
            catch (Exception ex)
            {
                if (lblMessage != null)
                {
                    lblMessage.Text = "An error occurred while processing your order: " + ex.Message;
                    lblMessage.Visible = true;
                }
                System.Diagnostics.Debug.WriteLine($"Error in btnConfirm_Click: {ex.Message}");
            }
        }
    }
}