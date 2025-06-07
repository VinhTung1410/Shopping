using System;
using Shopping.Controller1;
using Shopping.Model1;

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
                        // Get user details including role
                        User user = userController.GetUserByUsername(username);
                        if (user != null)
                        {
                            if (!user.IsActive)
                            {
                                Response.Redirect("~/View/SuspensePage.aspx");
                                return;
                            }
                            // Store user information in session
                            Session["Username"] = username;
                            Session["UserID"] = user.UserID;
                            Session["RoleID"] = user.RoleID;
                            Session["EmployeeID"] = user.EmployeeID;

                            // Redirect based on role
                            if (user.RoleID == 1) // Admin role
                            {
                                Response.Redirect("~/Admin/Adminpage.aspx");
                            }
                            else
                            {
                                Response.Redirect("~/Default.aspx");
                            }
                        }
                        else
                        {
                            lblMessage.Text = "User information not found.";
                        }
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