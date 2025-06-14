<%@ Page Title="Product List" Language="C#" MasterPageFile="~/Admin/AdminLayout.master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Shopping.Admin.Product" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />
    <script src="https://cdn.ckeditor.com/4.22.1/standard/ckeditor.js"></script>
    <style>
        .required::after {
            content: "*";
            color: red;
            margin-left: 4px;
        }
        .valid-input::after {
            content: "";
        }
        .product-image {
            max-width: 150px;
            max-height: 150px;
            margin: 5px;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            object-fit: cover;
        }
        .image-container {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            margin-top: 10px;
            padding: 10px;
            background: #f8f9fa;
            border-radius: 8px;
            min-height: 100px;
        }
        .image-item {
            position: relative;
            display: inline-block;
            border: 1px solid #ddd;
            padding: 5px;
            background: white;
            border-radius: 5px;
        }
        .delete-image {
            position: absolute;
            top: -10px;
            right: -10px;
            background: #dc3545;
            color: white;
            border: 2px solid white;
            border-radius: 50%;
            width: 24px;
            height: 24px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            box-shadow: 0 2px 4px rgba(0,0,0,0.2);
        }
        .delete-image:hover {
            background: #c82333;
        }
        .grid-image-container {
            display: flex;
            flex-wrap: wrap;
            gap: 5px;
            justify-content: flex-start;
            padding: 5px;
            background: #f8f9fa;
            border-radius: 4px;
            min-height: 50px;
        }
        .grid-image {
            width: 80px;
            height: 80px;
            object-fit: cover;
            border-radius: 4px;
            border: 1px solid #ddd;
            padding: 2px;
            background: white;
        }
        .upload-zone {
            border: 2px dashed #ddd;
            padding: 20px;
            text-align: center;
            background: #f8f9fa;
            border-radius: 8px;
            margin-bottom: 10px;
        }
        .upload-zone:hover {
            border-color: #007bff;
            background: #e9ecef;
        }
        .image-preview {
            margin-top: 10px;
            display: none;
        }
        .image-count-badge {
            position: absolute;
            top: -8px;
            right: -8px;
            background: #17a2b8;
            color: white;
            border-radius: 50%;
            padding: 2px 6px;
            font-size: 12px;
            min-width: 20px;
            text-align: center;
        }
        .ck-editor__editable {
            min-height: 200px;
        }
        #cke_txtDescription {
            width: 100%;
            margin-bottom: 20px;
        }
    </style>

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
                        <%--<div class="mb-3">
                            <label for="txtProductID" class="form-label">Product ID</label>
                            <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control" ReadOnly="true" />
                            <asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />
                        </div>--%>
                        <div class="mb-3">
                            <label for="txtProductName" class="form-label required">Product Name</label>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                                ControlToValidate="txtProductName" 
                                ErrorMessage="This zone is mandatory to fill out" 
                                Display="Dynamic" 
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label for="txtUnitPrice" class="form-label required">Unit Price</label>
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" />
                            <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" 
                                ControlToValidate="txtUnitPrice" 
                                ErrorMessage="This zone is mandatory to fill out" 
                                Display="Dynamic" 
                                CssClass="text-danger" />
                            <asp:CompareValidator ID="cvUnitPrice" runat="server"
                                ControlToValidate="txtUnitPrice"
                                Type="Double"
                                Operator="GreaterThan"
                                ValueToCompare="0"
                                ErrorMessage="The value must be greater than 0"
                                Display="Dynamic"
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label for="txtUnitsInStock" class="form-label required">Units In Stock</label>
                            <asp:TextBox ID="txtUnitsInStock" runat="server" CssClass="form-control" TextMode="Number" />
                            <asp:RequiredFieldValidator ID="rfvUnitsInStock" runat="server" 
                                ControlToValidate="txtUnitsInStock" 
                                ErrorMessage="This zone is mandatory to fill out" 
                                Display="Dynamic" 
                                CssClass="text-danger" />
                            <asp:CompareValidator ID="cvUnitsInStock" runat="server"
                                ControlToValidate="txtUnitsInStock"
                                Type="Integer"
                                Operator="GreaterThan"
                                ValueToCompare="-1"
                                ErrorMessage="The value must be positive"
                                Display="Dynamic"
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label for="txtDescription" class="form-label">Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Product Images</label>
                            <div class="upload-zone">
                                <asp:FileUpload ID="fuProductImages" runat="server" CssClass="form-control" AllowMultiple="true" accept=".jpg,.jpeg" />
                                <small class="text-muted d-block mt-2">Accepted formats: JPG only, Max size: 25MB, Max dimensions: 1500x1500</small>
                            </div>
                            <asp:Label ID="lblImageError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                            
                            <div class="image-container">
                                <asp:Repeater ID="rptProductImages" runat="server" OnItemCommand="rptProductImages_ItemCommand">
                                    <ItemTemplate>
                                        <div class="image-item">
                                            <asp:Image ID="imgProduct" runat="server" 
                                                ImageUrl='<%# ResolveUrl("~/Image/" + Eval("ImageUrl")) %>' 
                                                CssClass="product-image" 
                                                AlternateText='<%# Eval("OriginalFileName") %>'
                                                ToolTip='<%# Eval("OriginalFileName") %>' />
                                            <asp:LinkButton ID="btnDeleteImage" runat="server" 
                                                CommandName="DeleteImage" 
                                                CommandArgument='<%# Eval("ProductImageID") %>'
                                                CssClass="delete-image"
                                                OnClientClick="return confirm('Are you sure you want to delete this image?');">
                                                <i class="bi bi-x"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="mb-3">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="return BeforeSubmit();" />
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
                AutoGenerateColumns="False" OnRowCommand="gvProducts_RowCommand" OnRowDataBound="gvProducts_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="UnitsInStock" HeaderText="Units In Stock" />
                    <asp:TemplateField HeaderText="Images">
                        <ItemTemplate>
                            <div class="grid-image-container position-relative">
                                <asp:Repeater ID="rptThumbnails" runat="server">
                                    <ItemTemplate>
                                        <div class="image-item">
                                            <asp:Image ID="imgProduct" runat="server" 
                                                ImageUrl='<%# ResolveUrl("~/Image/" + Eval("ImageUrl")) %>' 
                                                CssClass="grid-image" 
                                                AlternateText='<%# Eval("OriginalFileName") %>'
                                                ToolTip='<%# Eval("OriginalFileName") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblImageCount" runat="server" CssClass="image-count-badge" Visible='<%# ((ICollection<Shopping.Model1.ProductImage>)((Shopping.Model1.Product)Container.DataItem).ProductImages).Count > 0 %>'>
                                    <%# ((ICollection<Shopping.Model1.ProductImage>)((Shopping.Model1.Product)Container.DataItem).ProductImages).Count %>
                                </asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
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

<asp:Content ID="Content2" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            // Initialize CKEditor with basic configuration
            if (CKEDITOR.instances['<%= txtDescription.ClientID %>']) {
                CKEDITOR.instances['<%= txtDescription.ClientID %>'].destroy();
            }
            
            CKEDITOR.replace('<%= txtDescription.ClientID %>', {
                height: '300px',
                toolbar: [
                    ['Source'],
                    ['Bold', 'Italic', 'Underline', 'Strike'],
                    ['NumberedList', 'BulletedList'],
                    ['Outdent', 'Indent'],
                    ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
                    ['Link', 'Unlink'],
                    ['Image', 'Table'],
                    ['Format', 'Font', 'FontSize'],
                    ['TextColor', 'BGColor'],
                    ['Maximize']
                ],
                removeButtons: '',
                language: 'en',
                removePlugins: 'elementspath',
                resize_enabled: false
            });

            function updateAsterisk(input) {
                var label = $("label[for='" + input.id + "']");
                if ($(input).val()) {
                    label.addClass("valid-input");
                } else {
                    label.removeClass("valid-input");
                }
            }

            <%--$("#<%= txtProductID.ClientID %>").on("input", function() { updateAsterisk(this); });--%>
            $("#<%= txtProductName.ClientID %>").on("input", function() { updateAsterisk(this); });
            $("#<%= txtUnitPrice.ClientID %>").on("input", function() { updateAsterisk(this); });
            $("#<%= txtUnitsInStock.ClientID %>").on("input", function() { updateAsterisk(this); });

            $("input[type='text'], input[type='number']").each(function() {
                updateAsterisk(this);
            });

            // File upload preview
            $("#<%= fuProductImages.ClientID %>").change(function () {
                var fileUpload = $(this)[0];
                if (fileUpload.files.length > 0) {
                    $(".upload-zone").css("border-color", "#28a745");
                } else {
                    $(".upload-zone").css("border-color", "#ddd");
                }
            });
        });

        // Add form submit handler to update CKEditor content
        function BeforeSubmit() {
            if (CKEDITOR.instances['<%= txtDescription.ClientID %>']) {
                CKEDITOR.instances['<%= txtDescription.ClientID %>'].updateElement();
            }
            return true;
        }
    </script>
</asp:Content>


