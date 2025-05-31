using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.Admin
{
    public partial class EmployeeList : Page
    {
        private readonly EmployeeController employeeController = new EmployeeController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
            }
        }

        private void LoadEmployees()
        {
            try
            {
                lblError.Visible = false;
                var employees = employeeController.GetAllEmployees();
                if (employees != null && employees.Count > 0)
                {
                    gvEmployees.DataSource = employees;
                    gvEmployees.DataBind();
                }
                else
                {
                    ShowError("No employees found in the database.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading employees: {ex.Message}");
                ShowError("An error occurred while loading employees: " + ex.Message);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/EmployeeEdit1.aspx");
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditEmployee")
            {
                int employeeId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Admin/EmployeeEdit1.aspx?id={employeeId}");
            }
            else if (e.CommandName == "ToggleStatus")
            {
                try
                {
                    int employeeId = Convert.ToInt32(e.CommandArgument);
                    var employee = employeeController.GetEmployeeById(employeeId);
                    if (employee != null)
                    {
                        bool newStatus = !employee.IsActive;
                        if (employeeController.ToggleEmployeeStatus(employeeId, newStatus))
                        {
                            LoadEmployees(); // Refresh the grid
                        }
                        else
                        {
                            ShowError("Failed to update employee status.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error toggling employee status: {ex.Message}");
                    ShowError("An error occurred while updating employee status.");
                }
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}