<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Shopping.View.Login" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow-lg border-0">
                    <div class="card-header bg-dark text-white text-center py-4">
                        <h2 class="font-weight-light mb-0">Đăng Nhập</h2>
                    </div>
                    <div class="card-body p-4">
                        <div class="form-floating mb-4">
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username" />
                            <asp:Label runat="server" AssociatedControlID="txtUsername" CssClass="form-label">Tên đăng nhập</asp:Label>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" 
                                CssClass="text-danger small" Display="Dynamic"
                                ErrorMessage="Vui lòng nhập tên đăng nhập." />
                        </div>

                        <div class="form-floating mb-4">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" />
                            <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Mật khẩu</asp:Label>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" 
                                CssClass="text-danger small" Display="Dynamic"
                                ErrorMessage="Vui lòng nhập mật khẩu." />
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnLogin" runat="server" Text="Đăng Nhập" 
                                CssClass="btn btn-dark btn-lg" OnClick="btnLogin_Click" />
                        </div>

                        <div class="text-center mt-3">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                        </div>
                    </div>
                    <div class="card-footer text-center py-3 bg-light">
                        <div class="small">
                            Chưa có tài khoản? <a href="Register.aspx" class="text-dark text-decoration-none">Đăng ký ngay</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .form-floating > .form-control {
            height: calc(3.5rem + 2px);
            padding: 1rem 0.75rem;
        }
        .form-floating > .form-control:focus {
            border-color: #333;
            box-shadow: 0 0 0 0.25rem rgba(33, 33, 33, 0.25);
        }
        .form-floating > .form-label {
            padding: 1rem 0.75rem;
        }
        .card {
            transition: all 0.3s ease-in-out;
        }
        .card:hover {
            transform: translateY(-5px);
        }
        .btn-dark {
            transition: all 0.3s ease;
        }
        .btn-dark:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        }
        .btn-dark:focus {
            box-shadow: 0 0 0 0.25rem rgba(33, 33, 33, 0.5);
        }
        a.text-dark:hover {
            color: #000 !important;
            text-decoration: underline !important;
        }
    </style>
</asp:Content> 