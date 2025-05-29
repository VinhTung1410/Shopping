<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Shopping.View.Login" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Login</h2>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtUsername">Username</asp:Label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="required"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" 
                CssClass="text-danger" ErrorMessage="Username is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtPassword">Password</asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required="required"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" 
                CssClass="text-danger" ErrorMessage="Password is required." />
        </div>

        <div class="form-group">
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
        </div>

        <div class="form-group">
            <p>Don't have an account? <asp:HyperLink runat="server" NavigateUrl="~/View/Register.aspx">Register here</asp:HyperLink></p>
        </div>
    </div>
</asp:Content> 