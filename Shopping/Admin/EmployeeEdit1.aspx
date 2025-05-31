<%@ Page Title="Employee Details" Language="C#" MasterPageFile="~/Admin/AdminLayout.master" AutoEventWireup="true" CodeBehind="EmployeeEdit1.aspx.cs" Inherits="Shopping.Admin.EmployeeEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <h2><asp:Literal ID="litPageTitle" runat="server" /></h2>
        
        <div class="row">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <!-- Account Information -->
                        <h4 class="card-title mb-4">Account Information</h4>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtUsername">Username</label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                        ControlToValidate="txtUsername" 
                                        ErrorMessage="Username is required" 
                                        Display="Dynamic" 
                                        CssClass="text-danger" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPassword">Password</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                            </div>
                        </div>

                        <!-- Personal Information -->
                        <h4 class="card-title mb-4">Personal Information</h4>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtFirstName">First Name</label>
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" 
                                        ControlToValidate="txtFirstName" 
                                        ErrorMessage="First Name is required" 
                                        Display="Dynamic" 
                                        CssClass="text-danger" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtLastName">Last Name</label>
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" 
                                        ControlToValidate="txtLastName" 
                                        ErrorMessage="Last Name is required" 
                                        Display="Dynamic" 
                                        CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtTitle">Title</label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtTitleOfCourtesy">Title of Courtesy</label>
                                    <asp:TextBox ID="txtTitleOfCourtesy" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtBirthDate">Birth Date</label>
                                    <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtHireDate">Hire Date</label>
                                    <asp:TextBox ID="txtHireDate" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                        </div>

                        <!-- Contact Information -->
                        <h4 class="card-title mb-4">Contact Information</h4>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtEmail">Email</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                        ControlToValidate="txtEmail" 
                                        ErrorMessage="Invalid email format" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        Display="Dynamic" 
                                        CssClass="text-danger" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtHomePhone">Home Phone</label>
                                    <asp:TextBox ID="txtHomePhone" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group mb-3">
                            <label for="txtAddress">Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtCity">City</label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtRegion">Region</label>
                                    <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPostalCode">Postal Code</label>
                                    <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtCountry">Country</label>
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtExtension">Extension</label>
                                    <asp:TextBox ID="txtExtension" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="ddlReportsTo">Reports To</label>
                                    <asp:DropDownList ID="ddlReportsTo" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Select Manager" Value="" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <!-- Role and Status -->
                        <h4 class="card-title mb-4">Role and Status</h4>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="ddlRole">Role</label>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Select Role" Value="" />
                                        <asp:ListItem Text="Admin" Value="1" Selected="false" />
                                        <asp:ListItem Text="User" Value="2" Selected="true" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" 
                                        ControlToValidate="ddlRole" 
                                        InitialValue=""
                                        ErrorMessage="Role is required" 
                                        Display="Dynamic" 
                                        CssClass="text-danger" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="chkIsActive">Status</label>
                                    <div>
                                        <asp:CheckBox ID="chkIsActive" runat="server" Text="Active" CssClass="form-check-input" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Read Only Information -->
                        <asp:Panel ID="pnlReadOnly" runat="server" CssClass="mb-3" Visible="false">
                            <h4 class="card-title mb-4">System Information</h4>
                            <div class="row">
                                <div class="col-md-6">
                                    <p><strong>Created Date:</strong> <asp:Literal ID="litCreatedDate" runat="server" /></p>
                                </div>
                                <div class="col-md-6">
                                    <p><strong>Last Update:</strong> <asp:Literal ID="litLastUpdate" runat="server" /></p>
                                </div>
                            </div>
                        </asp:Panel>

                        <!-- Error Messages -->
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" Visible="false" />

                        <!-- Buttons -->
                        <div class="mt-4">
                            <asp:Button ID="btnBack" runat="server" Text="Back to List" 
                                CssClass="btn btn-secondary" OnClick="btnBack_Click" CausesValidation="false" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" 
                                CssClass="btn btn-primary ms-2" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>