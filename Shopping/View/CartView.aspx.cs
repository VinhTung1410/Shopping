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
        private Dictionary<string, decimal> validCouponCodes = new Dictionary<string, decimal>
        {
            { "SAVE5", 5m },
            { "DISCOUNT10", 10m }
        };

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

                    Session["BasePrice"] = totalPrice;

                    if (litItemCount != null) litItemCount.Text = totalItems.ToString();
                    if (litSummaryItemCount != null) litSummaryItemCount.Text = totalItems.ToString();
                    if (litSummaryTotalPrice != null) litSummaryTotalPrice.Text = totalPrice.ToString("N0") + "€";
                    if (lblTotalAmount != null) lblTotalAmount.Text = totalPrice.ToString("N0") + "€";
                    if (lblFinalTotal != null) lblFinalTotal.Text = totalPrice.ToString("N0") + "€";

                    decimal discountAmount = 0;
                    if (Session["DiscountAmount"] != null)
                    {
                        discountAmount = Convert.ToDecimal(Session["DiscountAmount"]);
                    }

                    decimal shippingCost = 0;
                    if (Session["ShippingCost"] != null)
                    {
                        shippingCost = Convert.ToDecimal(Session["ShippingCost"]);
                    }

                    decimal finalTotal = totalPrice + shippingCost - discountAmount;
                    if (lblFinalTotal != null) lblFinalTotal.Text = finalTotal.ToString("N0") + "€";

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
            }
        }

        protected void btnApplyCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                string couponCode = txtCouponCode.Value?.Trim().ToUpper();
                
                if (string.IsNullOrEmpty(couponCode))
                {
                    lblCouponMessage.Text = "Please enter a coupon code.";
                    lblCouponMessage.Visible = true;
                    return;
                }

                if (validCouponCodes.TryGetValue(couponCode, out decimal discountAmount))
                {
                    Session["DiscountAmount"] = discountAmount;
                    Session["CouponCode"] = couponCode;

                    decimal basePrice = Convert.ToDecimal(Session["BasePrice"]);
                    decimal shippingCost = Session["ShippingCost"] != null ? Convert.ToDecimal(Session["ShippingCost"]) : 0;

                    decimal finalTotal = basePrice + shippingCost - discountAmount;

                    lblFinalTotal.Text = finalTotal.ToString("N0") + "€";
                    lblCouponMessage.Text = $"Coupon applied successfully! You saved {discountAmount}€";
                    lblCouponMessage.CssClass = "text-success mt-2";
                    lblCouponMessage.Visible = true;

                    Session["FinalTotal"] = finalTotal;
                }
                else
                {
                    lblCouponMessage.Text = "Invalid coupon code.";
                    lblCouponMessage.CssClass = "text-danger mt-2";
                    lblCouponMessage.Visible = true;
                    Session["DiscountAmount"] = 0;
                    Session["CouponCode"] = null;

                    decimal basePrice = Convert.ToDecimal(Session["BasePrice"]);
                    decimal shippingCost = Session["ShippingCost"] != null ? Convert.ToDecimal(Session["ShippingCost"]) : 0;
                    decimal finalTotal = basePrice + shippingCost;
                    lblFinalTotal.Text = finalTotal.ToString("N0") + "€";
                    Session["FinalTotal"] = finalTotal;
                }

                discountUpdatePanel.Update();
            }
            catch (Exception ex)
            {
                lblCouponMessage.Text = "An error occurred while applying the coupon.";
                lblCouponMessage.CssClass = "text-danger mt-2";
                lblCouponMessage.Visible = true;
                discountUpdatePanel.Update();
            }
        }

        [System.Web.Services.WebMethod]
        public static string UpdateTotalWithShipping(decimal shippingCost)
        {
            try
            {
                decimal basePrice = 0;
                if (HttpContext.Current.Session["BasePrice"] != null)
                {
                    basePrice = Convert.ToDecimal(HttpContext.Current.Session["BasePrice"]);
                }

                decimal discountAmount = 0;
                if (HttpContext.Current.Session["DiscountAmount"] != null)
                {
                    discountAmount = Convert.ToDecimal(HttpContext.Current.Session["DiscountAmount"]);
                }

                HttpContext.Current.Session["ShippingCost"] = shippingCost;

                decimal newTotal = basePrice + shippingCost - discountAmount;

                HttpContext.Current.Session["FinalTotal"] = newTotal;

                return newTotal.ToString("N0") + "€";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
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
                var shippingSelect = Request.Form["shippingSelect"];
                decimal shippingCost = 0;
                if (!string.IsNullOrEmpty(shippingSelect))
                {
                    shippingCost = Convert.ToDecimal(shippingSelect);
                }

                Session["ShippingCost"] = shippingCost;

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

                decimal basePrice = cartItems.Sum(item => item.Quantity * item.UnitPrice);
                Session["BasePrice"] = basePrice;

                decimal discountAmount = 0;
                string couponCode = null;
                if (Session["DiscountAmount"] != null)
                {
                    discountAmount = Convert.ToDecimal(Session["DiscountAmount"]);
                    couponCode = Session["CouponCode"]?.ToString();
                }

                decimal finalTotal = basePrice + shippingCost - discountAmount;
                Session["FinalTotal"] = finalTotal;

                lblTotalAmount.Text = basePrice.ToString("N0") + "€";
                lblFinalTotal.Text = finalTotal.ToString("N0") + "€";

                cartController.CompleteOrder(employeeId, finalTotal, couponCode, discountAmount);

                Session.Remove("DiscountAmount");
                Session.Remove("CouponCode");
                Session.Remove("ShippingCost");
                Session.Remove("BasePrice");
                Session.Remove("FinalTotal");

                Response.Redirect("~/View/OrderSuccess.aspx");
            }
            catch (Exception ex)
            {
                if (lblMessage != null)
                {
                    lblMessage.Text = "An error occurred while processing your order: " + ex.Message;
                    lblMessage.Visible = true;
                }
            }
        }
    }
}