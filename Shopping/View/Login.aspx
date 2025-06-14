<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Shopping.View.Login" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-5">
                <div class="card shadow-lg border-0 rounded-3 bg-white">
                    <div class="card-header bg-black text-white text-center py-4 border-0">
                        <h2 class="fw-bold mb-1">Welcome Back</h2>
                        <p class="mb-0 mt-2 text-light-emphasis">Login to your account</p>
                    </div>
                    <div class="card-body p-4">
                        <div class="form-floating mb-4">
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control border-0 bg-light" placeholder="Username" />
                            <asp:Label runat="server" AssociatedControlID="txtUsername" CssClass="form-label text-dark">Username</asp:Label>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" 
                                CssClass="text-danger small" Display="Dynamic"
                                ErrorMessage="Please enter username" />
                        </div>

                        <div class="form-floating mb-4">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control border-0 bg-light" placeholder="Password" />
                            <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label text-dark">Password</asp:Label>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" 
                                CssClass="text-danger small" Display="Dynamic"
                                ErrorMessage="Please enter password" />
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnLogin" runat="server" Text="Sign In" 
                                CssClass="btn btn-dark btn-lg fw-semibold" OnClick="btnLogin_Click" />
                        </div>

                        <div class="text-center mt-3">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                        </div>
                    </div>
                    <div class="card-footer text-center py-3 bg-light border-0">
                        <div class="small text-dark">
                            Don't have an account? 
                            <a href="Register.aspx" class="text-dark fw-bold text-decoration-none">Sign up now</a>
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
    </style>
</asp:Content> 