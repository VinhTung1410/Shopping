<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Shopping.View.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                <div class="card shadow-lg border-0 rounded-lg">
                    <div class="card-header bg-dark text-white text-center py-4">
                        <h2 class="font-weight-light mb-0">Đăng Ký Tài Khoản</h2>
                    </div>
                    <div class="card-body p-4">
                        <!-- Personal Information Section -->
                        <div class="border-bottom mb-4 pb-3">
                            <h5 class="text-secondary mb-3">Thông Tin Cá Nhân</h5>
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <div class="form-floating">
                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Họ" />
                                        <asp:Label runat="server" AssociatedControlID="txtFirstName" CssClass="form-label">Họ</asp:Label>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName"
                                            CssClass="text-danger small" Display="Dynamic" 
                                            ErrorMessage="Vui lòng nhập họ." />
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="form-floating">
                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Tên" />
                                        <asp:Label runat="server" AssociatedControlID="txtLastName" CssClass="form-label">Tên</asp:Label>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName"
                                            CssClass="text-danger small" Display="Dynamic" 
                                            ErrorMessage="Vui lòng nhập tên." />
                                    </div>
                                </div>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                    Rows="3" Style="height: auto;" placeholder="Địa chỉ" />
                                <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="form-label">Địa chỉ</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Vui lòng nhập địa chỉ." />
                            </div>
                        </div>

                        <!-- Account Information Section -->
                        <div class="mb-4">
                            <h5 class="text-secondary mb-3">Thông Tin Tài Khoản</h5>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Tên đăng nhập"
                                    AutoPostBack="true" OnTextChanged="txtUsername_TextChanged" />
                                <asp:Label runat="server" AssociatedControlID="txtUsername" CssClass="form-label">Tên đăng nhập</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Vui lòng nhập tên đăng nhập." />
                                <asp:CustomValidator ID="cvUsername" runat="server" 
                                    ControlToValidate="txtUsername"
                                    OnServerValidate="cvUsername_ServerValidate"
                                    CssClass="text-danger small" 
                                    ErrorMessage="Tên đăng nhập đã tồn tại." 
                                    Display="Dynamic" />
                            </div>

                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" 
                                    placeholder="Email" AutoPostBack="true" OnTextChanged="txtEmail_TextChanged" />
                                <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Email</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Vui lòng nhập email." />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Email không hợp lệ."
                                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" />
                                <asp:CustomValidator ID="cvEmail" runat="server" 
                                    ControlToValidate="txtEmail"
                                    OnServerValidate="cvEmail_ServerValidate"
                                    CssClass="text-danger small" 
                                    ErrorMessage="Email đã tồn tại." 
                                    Display="Dynamic" />
                            </div>

                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" 
                                    TextMode="Password" placeholder="Mật khẩu" />
                                <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Mật khẩu</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Vui lòng nhập mật khẩu." />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường và số."
                                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$" />
                            </div>

                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" 
                                    TextMode="Password" placeholder="Xác nhận mật khẩu" />
                                <asp:Label runat="server" AssociatedControlID="txtConfirmPassword" CssClass="form-label">Xác nhận mật khẩu</asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Vui lòng xác nhận mật khẩu." />
                                <asp:CompareValidator runat="server" ControlToCompare="txtPassword" 
                                    ControlToValidate="txtConfirmPassword"
                                    CssClass="text-danger small" Display="Dynamic" 
                                    ErrorMessage="Mật khẩu xác nhận không khớp." />
                            </div>
                        </div>

                        <asp:Label runat="server" ID="lblError" CssClass="text-danger d-block text-center mb-3" />

                        <div class="d-grid">
                            <asp:Button ID="btnRegister" runat="server" Text="Đăng Ký" 
                                CssClass="btn btn-primary btn-lg" OnClick="btnRegister_Click" />
                        </div>
                    </div>
                    <div class="card-footer text-center py-3 bg-light">
                        <div class="small">
                            Đã có tài khoản? <a href="Login.aspx" class="text-dark text-decoration-none">Đăng nhập</a>
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
        .form-floating > textarea.form-control {
            height: auto;
            min-height: calc(3.5rem + 2px);
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
        .btn-primary {
            background-color: #212121;
            border-color: #212121;
            transition: all 0.3s ease;
        }
        .btn-primary:hover {
            background-color: #000;
            border-color: #000;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        }
        .btn-primary:focus {
            background-color: #000;
            border-color: #000;
            box-shadow: 0 0 0 0.25rem rgba(33, 33, 33, 0.5);
        }
        .text-primary {
            color: #212121 !important;
        }
        .card-header.bg-dark {
            background-color: #212121 !important;
        }
    </style>
</asp:Content> 
