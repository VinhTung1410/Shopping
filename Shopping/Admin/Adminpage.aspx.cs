using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.Admin
{
    public partial class Adminpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    // Check if user is authenticated and has admin role
            //    if (Session["Username"] == null || Session["RoleID"].ToString() != "1")
            //    {
            //        Response.Redirect("~/View/Login.aspx");
            //        return;
            //    }

            //    // Set logged in user name
            //    lblLoggedInUser.Text = Session["Username"].ToString();

            //    LoadDashboardData();
            //    LoadRecentOrders();
            //}
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session variables
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("~/View/Login.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // TODO: Implement search functionality
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Add your search logic here
                // Example:
                // SearchOrders(searchTerm);
                // or
                // SearchProducts(searchTerm);
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                // TODO: Replace with actual data from your database
                // Example:
                // UserController userCtrl = new UserController();
                // lblUserCount.Text = userCtrl.GetTotalUsers().ToString();
                
                // Placeholder data for demonstration
                lblOrderCount.Text = "150";
                lblProductCount.Text = "75";
                lblRevenueAmount.Text = "$25,000";
                lblUserCount.Text = "45";
            }
            catch (Exception ex)
            {
                // Log error and show user-friendly message
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
            }
        }

        private void LoadRecentOrders()
        {
            try
            {
                // TODO: Replace with actual data from your database
                // Example:
                // OrderController orderCtrl = new OrderController();
                // gvOrders.DataSource = orderCtrl.GetRecentOrders(10);
                // gvOrders.DataBind();

                // Create sample data for demonstration
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("OrderID", typeof(int)),
                    new DataColumn("CustomerName", typeof(string)),
                    new DataColumn("OrderDate", typeof(DateTime)),
                    new DataColumn("TotalAmount", typeof(decimal)),
                    new DataColumn("Status", typeof(string))
                });

                // Add sample rows
                dt.Rows.Add(1, "John Doe", DateTime.Now.AddDays(-1), 299.99m, "Completed");
                dt.Rows.Add(2, "Jane Smith", DateTime.Now.AddDays(-2), 149.50m, "Processing");
                dt.Rows.Add(3, "Bob Johnson", DateTime.Now.AddDays(-3), 499.99m, "Shipped");

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
            catch (Exception ex)
            {
                // Log error and show user-friendly message
                System.Diagnostics.Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrder")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);
                // TODO: Implement view order details
                // Response.Redirect($"~/Admin/OrderDetails.aspx?id={orderId}");
            }
            else if (e.CommandName == "EditOrder")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);
                // TODO: Implement edit order
                // Response.Redirect($"~/Admin/EditOrder.aspx?id={orderId}");
            }
        }
    }
}