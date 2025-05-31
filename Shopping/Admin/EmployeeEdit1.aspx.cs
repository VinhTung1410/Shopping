using System;
using System.Web.UI;
using Shopping.Controller1;
using Shopping.Model1;

namespace Shopping.Admin
{
    public partial class EmployeeEdit : Page
    {
        private readonly EmployeeController employeeController = new EmployeeController();
        private int? employeeId = null;
        protected bool IsNewEmployee = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                employeeId = id;
                IsNewEmployee = false;
            }

            if (!IsPostBack)
            {
                if (employeeId.HasValue)
                {
                    LoadEmployee(employeeId.Value);
                    litPageTitle.Text = "Edit Employee";
                    pnlReadOnly.Visible = true;
                    txtPassword.ReadOnly = true;
                }
                else
                {
                    litPageTitle.Text = "Add New Employee";
                    pnlReadOnly.Visible = false;
                    chkIsActive.Checked = true;
                    txtPassword.ReadOnly = false;
                    txtPassword.Text = string.Empty;
                    // Set default role for new employee
                    ddlRole.SelectedValue = "2"; // Default to User role
                }

                // Load managers for ReportsTo dropdown
                LoadManagers();
            }
        }

        private void LoadManagers()
        {
            try
            {
                var managers = employeeController.GetAllEmployees();
                ddlReportsTo.Items.Clear();
                ddlReportsTo.Items.Add(new System.Web.UI.WebControls.ListItem("Select Manager", ""));
                foreach (var manager in managers)
                {
                    ddlReportsTo.Items.Add(new System.Web.UI.WebControls.ListItem(
                        $"{manager.FirstName} {manager.LastName}",
                        manager.EmployeeID.ToString()));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading managers: {ex.Message}");
            }
        }

        private void LoadEmployee(int id)
        {
            var employee = employeeController.GetEmployeeById(id);
            if (employee != null)
            {
                txtUsername.Text = employee.Username;
                txtUsername.Enabled = false;
                txtPassword.Text = employee.PasswordHash;
                
                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                txtTitle.Text = employee.Title;
                txtTitleOfCourtesy.Text = employee.TitleOfCourtesy;
                
                if (employee.BirthDate.HasValue)
                    txtBirthDate.Text = employee.BirthDate.Value.ToString("yyyy-MM-dd");
                if (employee.HireDate.HasValue)
                    txtHireDate.Text = employee.HireDate.Value.ToString("yyyy-MM-dd");

                txtEmail.Text = employee.Email;
                txtAddress.Text = employee.Address;
                txtCity.Text = employee.City;
                txtRegion.Text = employee.Region;
                txtPostalCode.Text = employee.PostalCode;
                txtCountry.Text = employee.Country;
                txtHomePhone.Text = employee.HomePhone;
                txtExtension.Text = employee.Extension;
                
                if (employee.ReportsTo.HasValue)
                    ddlReportsTo.SelectedValue = employee.ReportsTo.Value.ToString();

                // Set Role based on RoleID
                ddlRole.SelectedValue = employee.RoleID.ToString();
                chkIsActive.Checked = employee.IsActive;

                litCreatedDate.Text = employee.CreatedAt.ToString("MMM dd, yyyy HH:mm");
                litLastUpdate.Text = employee.UpdatedAt?.ToString("MMM dd, yyyy HH:mm") ?? "Never";
            }
            else
            {
                Response.Redirect("~/Admin/EmployeeList1.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Validate Role selection
                    if (string.IsNullOrEmpty(ddlRole.SelectedValue))
                    {
                        ShowError("Please select a role.");
                        return;
                    }

                    var employee = new Employee
                    {
                        Username = txtUsername.Text.Trim(),
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim(),
                        Title = string.IsNullOrEmpty(txtTitle.Text.Trim()) ? null : txtTitle.Text.Trim(),
                        TitleOfCourtesy = string.IsNullOrEmpty(txtTitleOfCourtesy.Text.Trim()) ? null : txtTitleOfCourtesy.Text.Trim(),
                        BirthDate = string.IsNullOrEmpty(txtBirthDate.Text) ? (DateTime?)null : DateTime.Parse(txtBirthDate.Text),
                        HireDate = string.IsNullOrEmpty(txtHireDate.Text) ? (DateTime?)null : DateTime.Parse(txtHireDate.Text),
                        Email = string.IsNullOrEmpty(txtEmail.Text.Trim()) ? null : txtEmail.Text.Trim(),
                        Address = string.IsNullOrEmpty(txtAddress.Text.Trim()) ? null : txtAddress.Text.Trim(),
                        City = string.IsNullOrEmpty(txtCity.Text.Trim()) ? null : txtCity.Text.Trim(),
                        Region = string.IsNullOrEmpty(txtRegion.Text.Trim()) ? null : txtRegion.Text.Trim(),
                        PostalCode = string.IsNullOrEmpty(txtPostalCode.Text.Trim()) ? null : txtPostalCode.Text.Trim(),
                        Country = string.IsNullOrEmpty(txtCountry.Text.Trim()) ? null : txtCountry.Text.Trim(),
                        HomePhone = string.IsNullOrEmpty(txtHomePhone.Text.Trim()) ? null : txtHomePhone.Text.Trim(),
                        Extension = string.IsNullOrEmpty(txtExtension.Text.Trim()) ? null : txtExtension.Text.Trim(),
                        ReportsTo = string.IsNullOrEmpty(ddlReportsTo.SelectedValue) ? (int?)null : int.Parse(ddlReportsTo.SelectedValue),
                        RoleID = int.Parse(ddlRole.SelectedValue), // Role is required and validated above
                        IsActive = chkIsActive.Checked
                    };

                    if (employeeId.HasValue)
                    {
                        // Update existing employee - keep existing password
                        employee.EmployeeID = employeeId.Value;
                        employee.PasswordHash = txtPassword.Text; // Keep existing password
                        if (employeeController.UpdateEmployee(employee))
                        {
                            Response.Redirect("~/Admin/EmployeeList1.aspx");
                        }
                        else
                        {
                            ShowError("Failed to update employee.");
                        }
                    }
                    else
                    {
                        // Create new employee - require password
                        if (string.IsNullOrEmpty(txtPassword.Text))
                        {
                            ShowError("Password is required for new employees.");
                            return;
                        }
                        employee.PasswordHash = txtPassword.Text;
                        int newId = employeeController.CreateEmployee(employee);
                        if (newId > 0)
                        {
                            Response.Redirect("~/Admin/EmployeeList1.aspx");
                        }
                        else
                        {
                            ShowError("Failed to create employee.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError("An error occurred: " + ex.Message);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/EmployeeList1.aspx");
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}