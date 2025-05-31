<%@ Page Title="Employee Management" Language="C#" MasterPageFile="~/Admin/AdminLayout.master" AutoEventWireup="true" CodeBehind="EmployeeList1.aspx.cs" Inherits="Shopping.Admin.EmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>Employee Management</h2>
            <asp:Button ID="btnAddNew" runat="server" Text="Add New Employee" CssClass="btn btn-dark" OnClick="btnAddNew_Click" />
        </div>

        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block mb-3" Visible="false" />

        <asp:GridView ID="gvEmployees" runat="server" CssClass="table table-striped table-bordered table-hover" 
            AutoGenerateColumns="False" DataKeyNames="EmployeeID" OnRowCommand="gvEmployees_RowCommand">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Username" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="City" HeaderText="City" />
                <asp:BoundField DataField="Country" HeaderText="Country" />
                <asp:BoundField DataField="RoleName" HeaderText="Role" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-danger" %>'>
                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-primary me-2" 
                            CommandName="EditEmployee" CommandArgument='<%# Eval("EmployeeID") %>'>
                            <i class="fas fa-edit"></i> Edit
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnToggleStatus" runat="server" 
                            CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn btn-sm btn-danger" : "btn btn-sm btn-success" %>'
                            CommandName="ToggleStatus" CommandArgument='<%# Eval("EmployeeID") %>'>
                            <i class='<%# Convert.ToBoolean(Eval("IsActive")) ? "fas fa-lock" : "fas fa-lock-open" %>'></i>
                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Activate" %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="text-center p-4">
                    <p class="text-muted">No employees found.</p>
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <style>
        .table {
            vertical-align: middle;
        }
        .btn-sm {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
        }
        .badge {
            font-size: 0.875rem;
            padding: 0.5em 0.75em;
        }
    </style>
</asp:Content> 