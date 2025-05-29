using System;
using Shopping.Controller1;

namespace Shopping.View
{
    public partial class Login : System.Web.UI.Page
    {
        private UserController userController = new UserController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing session
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string username = txtUsername.Text;
                    string password = txtPassword.Text;

                    if (userController.ValidateLogin(username, password))
                    {
                        Session["Username"] = username;
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid username or password.";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred: " + ex.Message;
                }
            }
        }
    }
} 