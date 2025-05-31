using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.View
{
    public partial class Register : System.Web.UI.Page
    {
        private UserController userController = new UserController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing session
                Session.Clear();
                // Clear any existing error messages
                lblError.Text = string.Empty;
                // Clear any existing validation messages
                cvUsername.Visible = false;
                cvEmail.Visible = false;
            }
        }

        protected void cvUsername_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string username = args.Value;
            if (!string.IsNullOrEmpty(username))
            {
                args.IsValid = !userController.IsUsernameExists(username);
            }
        }

        protected void cvEmail_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string email = args.Value;
            if (!string.IsNullOrEmpty(email))
            {
                args.IsValid = !userController.IsEmailExists(email);
            }
        }

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {
            cvUsername.Visible = true;
            Page.Validate("cvUsername");
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            cvEmail.Visible = true;
            Page.Validate("cvEmail");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Kiểm tra mật khẩu và xác nhận mật khẩu có khớp nhau
                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        lblError.Text = "Mật khẩu xác nhận không khớp với mật khẩu.";
                        return;
                    }

                    // Create new Employee
                    Employee employee = new Employee
                    {
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Address = txtAddress.Text
                    };

                    // Create new User
                    User user = new User
                    {
                        UserName = txtUsername.Text,
                        Password = txtPassword.Text, // Note: In production, this should be hashed
                        Email = txtEmail.Text,
                        RoleID = 2, // Default role for regular users
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    // Register the user
                    if (userController.RegisterUser(user, employee))
                    {
                        // Set success message
                        Session["Username"] = user.UserName;
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        lblError.Text = "Đăng ký không thành công. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Có lỗi xảy ra: " + ex.Message;
                }
            }
        }
    }
} 
