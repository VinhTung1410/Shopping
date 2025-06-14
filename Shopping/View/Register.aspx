<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Shopping.View.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                <div class="card shadow-lg border-0 rounded-3 bg-white">
                    <div class="card-header bg-black text-white text-center py-4 border-0">
                        <h2 class="fw-bold mb-1">Create Account</h2>
                        <p class="mb-0 mt-2 text-light-emphasis">Join us today</p>
                    </div>
                    <div class="card-body p-4">
                        <!-- Personal Information Section -->
                        <div class="border-bottom mb-4 pb-3">
                            <h5 class="text-dark fw-semibold mb-3">Personal Information</h5>
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <div class="form-floating">
                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control border-0 bg-light" placeholder="First Name" />
                                        <asp:Label runat="server" AssociatedControlID="txtFirstName" CssClass="form-label text-dark">First Name</asp:Label>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName"
                                            CssClass="text-danger small" Display="Dynamic" 
                                            ErrorMessage="Please enter first name" />
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="form-floating">
                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control border-0 bg-light" placeholder="Last Name" />
                                        <asp:Label runat="server" AssociatedControlID="txtLastName" CssClass="form-label text-dark">Last Name</asp:Label>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName"
                                            CssClass="text-danger small" Display="Dynamic" 
                                            ErrorMessage="Please enter last name" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control border-0 bg-light" TextMode="MultiLine" 
                                    Rows="3" Style="height: auto;" placeholder="Address" />
                                <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="form-label text-dark">Address</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Please enter address" />
                            </div>
                        </div>

                        <!-- Account Information Section -->
                        <div class="mb-4">
                            <h5 class="text-dark fw-semibold mb-3">Account Information</h5>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control border-0 bg-light" placeholder="Username"
                                    AutoPostBack="true" OnTextChanged="txtUsername_TextChanged" />
                                <asp:Label runat="server" AssociatedControlID="txtUsername" CssClass="form-label text-dark">Username</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Please enter username" />
                                <asp:CustomValidator ID="cvUsername" runat="server" 
                                    ControlToValidate="txtUsername"
                                    OnServerValidate="cvUsername_ServerValidate"
                                    CssClass="text-danger small" 
                                    ErrorMessage="The username already exists" 
                                    Display="Dynamic" />
                            </div>

                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control border-0 bg-light" TextMode="Email" 
                                    placeholder="Email" AutoPostBack="true" OnTextChanged="txtEmail_TextChanged" />
                                <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label text-dark">Email</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Please enter email" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Invalid email"
                                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" />
                                <asp:CustomValidator ID="cvEmail" runat="server" 
                                    ControlToValidate="txtEmail"
                                    OnServerValidate="cvEmail_ServerValidate"
                                    CssClass="text-danger small" 
                                    ErrorMessage="The email already exists" 
                                    Display="Dynamic" />
                            </div>

                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control border-0 bg-light" 
                                    TextMode="Password" placeholder="Password" />
                                <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label text-dark">Password</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Please enter password" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Password must be at least 6 characters, including uppercase, lowercase and numbers"
                                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$" />
                            </div>

                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control border-0 bg-light" 
                                    TextMode="Password" placeholder="Confirm Password" />
                                <asp:Label runat="server" AssociatedControlID="txtConfirmPassword" CssClass="form-label text-dark">Confirm Password</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Please enter confirm password" />
                                <asp:CompareValidator runat="server" ControlToCompare="txtPassword" 
                                    ControlToValidate="txtConfirmPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Confirmation password does not match" />
                            </div>
                        </div>

                        <asp:Label runat="server" ID="lblError" CssClass="text-danger d-block text-center mb-3" />

                        <div class="d-grid">
                            <asp:Button ID="btnRegister" runat="server" Text="Create Account" 
                                CssClass="btn btn-dark btn-lg fw-semibold" OnClick="btnRegister_Click" />
                        </div>
                    </div>
                    <div class="card-footer text-center py-3 bg-light border-0">
                        <div class="small text-dark">
                            Already have an account? 
                            <a href="Login.aspx" class="text-dark fw-bold text-decoration-none">Sign in</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        body {
            background-color: #f8f9fa !important;
        }
        .form-floating > .form-control {
            height: calc(3.5rem + 2px);
            padding: 1rem 0.75rem;
            transition: all 0.3s ease;
        }
        .form-floating > .form-control:focus {
            box-shadow: 0 0 0 0.25rem rgba(0, 0, 0, 0.1);
            background-color: #fff !important;
        }
        .form-floating > textarea.form-control {
            height: auto;
            min-height: calc(3.5rem + 2px);
        }
        .form-floating > .form-label {
            color: #495057;
            font-weight: 500;
        }
        .btn-dark {
            transition: all 0.3s ease;
            padding: 0.8rem;
            letter-spacing: 0.5px;
        }
        .btn-dark:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
        .btn-dark:focus {
            box-shadow: 0 0 0 0.25rem rgba(0, 0, 0, 0.25);
        }
        .card {
            border-radius: 1rem;
            transition: all 0.3s ease-in-out;
        }
        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 1rem 2rem rgba(0, 0, 0, 0.1) !important;
        }
        a.text-dark:hover {
            color: #000 !important;
            text-decoration: underline !important;
        }
        .card-header {
            border-radius: 1rem 1rem 0 0 !important;
        }
        .card-footer {
            border-radius: 0 0 1rem 1rem !important;
        }
        .text-danger {
            text-align: center;
            display: block;
        }
        .text-light-emphasis {
            color: rgba(255, 255, 255, 0.8) !important;
        }
        .border-bottom {
            border-color: rgba(0, 0, 0, 0.1) !important;
        }
    </style>
</asp:Content> 
