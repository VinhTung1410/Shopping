<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Shopping.View.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow">
                    <div class="card-body">
                        <h2 class="card-title text-center mb-4">Đăng ký tài khoản</h2>
                        
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtFirstName" CssClass="form-label">Họ</asp:Label>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName"
                                CssClass="text-danger" ErrorMessage="Vui lòng nhập họ." />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtLastName" CssClass="form-label">Tên</asp:Label>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName"
                                CssClass="text-danger" ErrorMessage="Vui lòng nhập tên." />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="form-label">Địa chỉ</asp:Label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress"
                                CssClass="text-danger" ErrorMessage="Vui lòng nhập địa chỉ." />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtUsername" CssClass="form-label">Tên đăng nhập</asp:Label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                                CssClass="text-danger" ErrorMessage="Vui lòng nhập tên đăng nhập." />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Email</asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                                CssClass="text-danger" ErrorMessage="Vui lòng nhập email." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
                                CssClass="text-danger" ErrorMessage="Email không hợp lệ."
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Mật khẩu</asp:Label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                                CssClass="text-danger" ErrorMessage="Vui lòng nhập mật khẩu." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword"
                                CssClass="text-danger" ErrorMessage="Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường và số."
                                ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$" />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtConfirmPassword" CssClass="form-label">Xác nhận mật khẩu</asp:Label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword"
                                CssClass="text-danger" ErrorMessage="Vui lòng xác nhận mật khẩu." />
                            <asp:CompareValidator runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"
                                CssClass="text-danger" ErrorMessage="Mật khẩu xác nhận không khớp." />
                        </div>

                        <div class="mb-3">
                            <asp:Label runat="server" ID="lblError" CssClass="text-danger" />
                        </div>

                        <div class="text-center">
                            <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" 
                                CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                        </div>

                        <div class="mt-3 text-center">
                            <p>Đã có tài khoản? <a href="Login.aspx">Đăng nhập</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 
