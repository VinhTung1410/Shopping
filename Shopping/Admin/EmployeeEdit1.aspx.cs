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
        private Employee currentEmployee = null;

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
                ShowError("Failed to load managers list.");
            }
        }

        private void LoadEmployee(int id)
        {
            currentEmployee = employeeController.GetEmployeeById(id);
            if (currentEmployee != null)
            {
                txtUsername.Text = currentEmployee.Username;
                txtUsername.Enabled = false;
                txtPassword.Text = currentEmployee.PasswordHash;
                
                txtFirstName.Text = currentEmployee.FirstName;
                txtLastName.Text = currentEmployee.LastName;
                txtTitle.Text = currentEmployee.Title;
                txtTitleOfCourtesy.Text = currentEmployee.TitleOfCourtesy;
                
                if (currentEmployee.BirthDate.HasValue)
                    txtBirthDate.Text = currentEmployee.BirthDate.Value.ToString("yyyy-MM-dd");
                if (currentEmployee.HireDate.HasValue)
                    txtHireDate.Text = currentEmployee.HireDate.Value.ToString("yyyy-MM-dd");

                txtEmail.Text = currentEmployee.Email;
                txtAddress.Text = currentEmployee.Address;
                txtCity.Text = currentEmployee.City;
                txtRegion.Text = currentEmployee.Region;
                txtPostalCode.Text = currentEmployee.PostalCode;
                txtCountry.Text = currentEmployee.Country;
                txtHomePhone.Text = currentEmployee.HomePhone;
                txtExtension.Text = currentEmployee.Extension;
                
                if (currentEmployee.ReportsTo.HasValue)
                    ddlReportsTo.SelectedValue = currentEmployee.ReportsTo.Value.ToString();

                // Set Role and Active status
                ddlRole.SelectedValue = currentEmployee.RoleID.ToString();
                chkIsActive.Checked = currentEmployee.IsActive;

                litCreatedDate.Text = currentEmployee.CreatedAt.ToString("MMM dd, yyyy HH:mm");
                litLastUpdate.Text = currentEmployee.UpdatedAt?.ToString("MMM dd, yyyy HH:mm") ?? "Never";
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

                    // Check if trying to deactivate last admin
                    if (!IsNewEmployee && currentEmployee != null)
                    {
                        bool isCurrentlyAdmin = currentEmployee.RoleID == 1;
                        bool wasActive = currentEmployee.IsActive;
                        bool willBeActive = chkIsActive.Checked;
                        int newRoleId = int.Parse(ddlRole.SelectedValue);

                        if (isCurrentlyAdmin && wasActive && (!willBeActive || newRoleId != 1))
                        {
                            // Count other active admins
                            var employees = employeeController.GetAllEmployees();
                            int activeAdminCount = 0;
                            foreach (var emp in employees)
                            {
                                if (emp.EmployeeID != currentEmployee.EmployeeID && 
                                    emp.RoleID == 1 && emp.IsActive)
                                {
                                    activeAdminCount++;
                                }
                            }

                            if (activeAdminCount == 0)
                            {
                                ShowError("Cannot deactivate or change role of the last active administrator.");
                                return;
                            }
                        }
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
                        RoleID = int.Parse(ddlRole.SelectedValue),
                        IsActive = chkIsActive.Checked
                    };

                    if (employeeId.HasValue)
                    {
                        // Update existing employee
                        employee.EmployeeID = employeeId.Value;
                        employee.PasswordHash = txtPassword.Text; // Keep existing password
                        
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Updating employee {employee.EmployeeID}");
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] New Role: {employee.RoleID}");
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] New IsActive: {employee.IsActive}");

                        if (employeeController.UpdateEmployee(employee))
                        {
                            Response.Redirect("~/Admin/EmployeeList1.aspx");
                        }
                        else
                        {
                            ShowError("Failed to update employee. Please check the values and try again.");
                        }
                    }
                    else
                    {
                        // Create new employee
                        if (string.IsNullOrEmpty(txtPassword.Text))
                        {
                            ShowError("Password is required for new employees.");
                            return;
                        }
                        employee.PasswordHash = txtPassword.Text;
                        
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Creating new employee");
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Role: {employee.RoleID}");
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] IsActive: {employee.IsActive}");

                        int newId = employeeController.CreateEmployee(employee);
                        if (newId > 0)
                        {
                            Response.Redirect("~/Admin/EmployeeList1.aspx");
                        }
                        else
                        {
                            ShowError("Failed to create employee. Please check the values and try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Error in btnSave_Click: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
                    ShowError($"An error occurred: {ex.Message}");
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
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Error shown to user: {message}");
        }
    }
}