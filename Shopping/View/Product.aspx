<%@ Page Title="Product List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Shopping.View.Product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />

    <div class="card shadow border-0 my-4">
        <div class="card-header bg-secondary bg-gradient ml-0 py-3">
            <div class="row">
                <div class="col-12 text-center">
                    <h2 class="text-white py-2">Product List</h2>
                </div>
            </div>
        </div>
        <div class="card-body p-4">
            <div class="row pb-3">
                <div class="col-6">
                    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="mb-3" Visible="false">
                        <div class="mb-3">
                            <label for="txtProductID" class="form-label">Product ID</label>
                            <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvProductID" runat="server" 
                                ControlToValidate="txtProductID" 
                                ErrorMessage="Product ID is required" 
                                Display="Dynamic" 
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label for="txtProductName" class="form-label">Product Name</label>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="txtQuantityPerUnit" class="form-label">Quantity Per Unit</label>
                            <asp:TextBox ID="txtQuantityPerUnit" runat="server" CssClass="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="txtUnitPrice" class="form-label">Unit Price</label>
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" />
                        </div>
                        <div class="mb-3">
                            <label for="txtUnitsInStock" class="form-label">Units In Stock</label>
                            <asp:TextBox ID="txtUnitsInStock" runat="server" CssClass="form-control" TextMode="Number" />
                        </div>
                        <div class="mb-3">
                            <label for="txtUnitsOnOrder" class="form-label">Units On Order</label>
                            <asp:TextBox ID="txtUnitsOnOrder" runat="server" CssClass="form-control" TextMode="Number" />
                        </div>
                        <div class="mb-3">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-6 text-end">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" CausesValidation="false">
                        <i class="bi bi-plus-circle"></i> Add
                    </asp:LinkButton>
                </div>
            </div>

            <asp:GridView ID="gvProducts" runat="server" CssClass="table table-bordered table-striped" 
                AutoGenerateColumns="False" OnRowCommand="gvProducts_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="QuantityPerUnit" HeaderText="Quantity Per Unit" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="UnitsInStock" HeaderText="Units In Stock" />
                    <asp:BoundField DataField="UnitsOnOrder" HeaderText="Units On Order" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div class="btn-group" role="group">
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-warning" 
                                    CommandName="EditProduct" CommandArgument='<%# Eval("ProductID") %>'>
                                    <i class="bi bi-pencil"></i> Edit
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" 
                                    CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this product?');">
                                    <i class="bi bi-trash"></i> Delete
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
    </asp:Panel>
</asp:Content>


