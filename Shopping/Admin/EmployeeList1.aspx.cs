using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.Admin
{
    public partial class EmployeeList : Page
    {
        private readonly EmployeeController1 employeeController = new EmployeeController1();

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
                    System.Diagnostics.Debug.WriteLine($"Attempting to toggle status for employee ID: {employeeId}");

                    var employee = employeeController.GetEmployeeById(employeeId);
                    if (employee != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Current employee status - IsActive: {employee.IsActive}, Role: {employee.RoleName}");
                        bool newStatus = !employee.IsActive;
                        
                        try
                        {
                            bool result = employeeController.ToggleEmployeeStatus(employeeId, newStatus);
                            System.Diagnostics.Debug.WriteLine($"Toggle result: {result}");
                            
                            if (result)
                            {
                                LoadEmployees(); // Refresh the grid
                                ShowSuccess($"Successfully {(newStatus ? "activated" : "deactivated")} employee {employee.FirstName} {employee.LastName}.");
                            }
                            else
                            {
                                ShowError($"Failed to {(newStatus ? "activate" : "deactivate")} employee {employee.FirstName} {employee.LastName}. Please try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error during toggle: {ex.Message}");
                            ShowError(ex.Message);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Employee not found");
                        ShowError("Employee not found.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unexpected error: {ex.Message}");
                    ShowError("An unexpected error occurred: " + ex.Message);
                }
            }
        }

        private void ShowError(string message)
        {
            lblError.CssClass = "alert alert-danger d-block mb-3";
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void ShowSuccess(string message)
        {
            lblError.CssClass = "alert alert-success d-block mb-3";
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}