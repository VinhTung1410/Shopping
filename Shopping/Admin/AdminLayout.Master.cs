using System;
using System.Web.UI;
using static System.Collections.Specialized.BitVector32;

namespace Shopping.Admin
{
    public partial class AdminLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    // Check if user is authenticated and has admin rights
            //    if (Session["UserID"] == null || Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            //    {
            //        Response.Redirect("~/Login.aspx");
            //    }

            //    // Set logged in user name
            //    if (Session["UserName"] != null)
            //    {
            //        lblLoggedInUser.Text = Session["UserName"].ToString();
            //    }
            //}
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Implement search functionality
            string searchQuery = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                Response.Redirect($"~/Admin/Search.aspx?q={Server.UrlEncode(searchQuery)}");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session and redirect to login
            Session.Clear();
            Response.Redirect("~/View/Login.aspx");
        }
    }
}