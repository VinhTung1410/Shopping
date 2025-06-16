<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartView.aspx.cs" Inherits="Shopping.View.WebForm1" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    <style>
        body {
            background-color: #f8f9fa; /* Light gray background */
        }

        .card-registration-2 {
            box-shadow: 0 4px 8px rgba(0,0,0,0.1); /* Subtle shadow */
        }

        .p-5 {
            padding: 3rem !important;
        }

        .cart-item-image {
            width: 100px;
            height: 100px;
            object-fit: cover;
            border-radius: 8px;
        }

        .quantity-control .btn {
            width: 38px;
            height: 38px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 0.25rem; /* Match Bootstrap's default button radius */
        }

        .quantity-control .form-control {
            width: 60px;
            text-align: center;
            border-left: none; /* Remove left border */
            border-right: none; /* Remove right border */
            border-radius: 0;
        }

        .quantity-control .form-control:focus {
            box-shadow: none;
        }

        .total-summary {
            background-color: #f8f9fa; /* Light gray for summary background */
            border-left: 1px solid #dee2e6; /* Separator line */
        }

        .total-summary h3, .total-summary h5 {
            color: #212529; /* Dark text for black/white theme */
        }

        .price-text {
            color: #dc3545; /* Red for emphasis, or black if strictly black/white */
            font-weight: bold;
            font-size: 1.25rem;
        }

        .text-muted-dark {
            color: #6c757d !important; /* Darker muted text */
        }

        .btn-outline-secondary {
            color: #343a40; /* Dark text for outline buttons */
            border-color: #343a40;
        }

        .btn-outline-secondary:hover {
            background-color: #343a40;
            color: #fff;
        }

        .btn-remove {
            color: #dc3545; /* Red for remove, or black for black/white */
        }

        .btn-remove:hover {
            color: #bd2130;
        }
    </style>
    <section class="h-100 h-custom" style="background-color: #fff;">
        <div class="container py-5 h-100">
            <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col-12">
                    <div class="card card-registration card-registration-2" style="border-radius: 15px; border: 1px solid #dee2e6;">
                        <div class="card-body p-0">
                            <div class="row g-0">
                                <div class="col-lg-8">
                                    <div class="p-5">
                                        <asp:UpdatePanel ID="cartUpdatePanel" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                        <div class="d-flex justify-content-between align-items-center mb-5">
                                                    <h1 class="fw-bold mb-0 text-black">Shopping Cart</h1>
                                                    <h6 class="mb-0 text-muted-dark">Items: <asp:Literal ID="litItemCount" runat="server"></asp:Literal></h6>
                                        </div>
                                        <hr class="my-4">

                                                <asp:Repeater ID="rptShoppingCart" runat="server" OnItemCommand="rptShoppingCart_ItemCommand">
                                            <ItemTemplate>
                                                <div class="row mb-4 d-flex justify-content-between align-items-center">
                                                    <div class="col-md-2 col-lg-2 col-xl-2">
                                                                <img src="<%# ResolveUrl("~/Image/" + Eval("ImageUrl")) %>" class="img-fluid rounded-3 cart-item-image" alt="Product Image" />
                                                    </div>
                                                    <div class="col-md-3 col-lg-3 col-xl-3">
                                                                <h6 class="text-muted-dark">Product</h6>
                                                                <h6 class="mb-0 text-black"><%# Eval("ProductName") %></h6>
                                                    </div>
                                                            <div class="col-md-3 col-lg-3 col-xl-2 d-flex quantity-control">
                                                                <asp:LinkButton ID="btnDecreaseQty" runat="server" CommandName="Decrease" CommandArgument='<%# Eval("ProductID").ToString() %>' CssClass="btn btn-outline-secondary">
                                                            <i class="fas fa-minus"></i>
                                                                </asp:LinkButton>
                                                                <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control form-control-sm" TextMode="Number" ReadOnly="true" data-productid='<%# Eval("ProductID").ToString() %>' data-unitsinstock='<%# Eval("UnitsInStock").ToString() %>' />
                                                                <asp:LinkButton ID="btnIncreaseQty" runat="server" CommandName="Increase" CommandArgument='<%# Eval("ProductID").ToString() %>' CssClass="btn btn-outline-secondary">
                                                            <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-3 col-lg-2 col-xl-2 offset-lg-1">
                                                                <h6 class="mb-0 price-text"><%# Eval("UnitPrice", "{0:N0}$") %></h6>
                                                                <span class="text-danger" style="font-size: 0.8em;" id="qtyError_<%# Eval("ProductID") %>"></span>
                                                    </div>
                                                    <div class="col-md-1 col-lg-1 col-xl-1 text-end">
                                                                <asp:LinkButton ID="btnRemoveItem" runat="server" CommandName="Remove" CommandArgument='<%# Eval("ProductID") %>' CssClass="text-muted-dark btn-remove">
                                                                    <i class="fas fa-times"></i>
                                                                </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <hr class="my-4">
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <div class="pt-5">
                                            <h6 class="mb-0">
                                                        <asp:HyperLink ID="hlBackToShop" runat="server" NavigateUrl="~/Default.aspx" CssClass="text-body text-black">
                                                    <i class="fas fa-long-arrow-alt-left me-2"></i>Back to shop
                                                </asp:HyperLink>
                                            </h6>
                                        </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rptShoppingCart" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="col-lg-4 total-summary">
                                    <div class="p-5">
                                        <h3 class="fw-bold mb-5 mt-2 pt-1 text-black">Summary</h3>
                                        <hr class="my-4">

                                        <div class="d-flex justify-content-between mb-4">
                                            <h5 class="text-uppercase text-black">items <asp:Literal ID="litSummaryItemCount" runat="server"></asp:Literal></h5>
                                            <h5 class="price-text"><asp:Literal ID="litSummaryTotalPrice" runat="server"></asp:Literal></h5>
                                        </div>

                                        <h5 class="text-uppercase mb-3 text-black">Shipping</h5>

                                        <asp:UpdatePanel ID="shippingUpdatePanel" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="mb-4 pb-2">
                                                    <select class="form-select" id="shippingSelect" runat="server" onchange="updateTotalPrice()">
                                                        <option value="0">Select shipping method</option>
                                                        <option value="5">Standard-Delivery- €5.00</option>
                                                        <option value="10">Express shipping- €10.00</option>             
                                                    </select>
                                                    <span id="shippingError" class="text-danger" style="display:none;">Please select a shipping method</span>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="d-flex justify-content-between mb-5">
                                            <h5 class="text-uppercase text-black">Total price</h5>
                                            <h5 class="price-text">
                                                <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                                            </h5>
                                        </div>

                                        <div class="d-flex justify-content-between mb-5">
                                            <h5 class="text-uppercase text-black">Final Total</h5>
                                            <h5 class="price-text">
                                                <asp:Label ID="lblFinalTotal" runat="server" Text=""></asp:Label>
                                            </h5>
                                        </div>

                                        <h5 class="text-uppercase mb-3 text-black">Give code</h5>

                                        <asp:UpdatePanel ID="discountUpdatePanel" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="mb-5">
                                                    <div class="form-outline d-flex">
                                                        <input type="text" id="txtCouponCode" class="form-control form-control-lg" runat="server" />
                                                        <asp:Button ID="btnApplyCoupon" runat="server" Text="Apply" CssClass="btn btn-dark ms-2" OnClick="btnApplyCoupon_Click" />
                                                    </div>
                                                    <asp:Label ID="lblCouponMessage" runat="server" CssClass="d-block mt-2" Visible="false"></asp:Label>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnApplyCoupon" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <hr class="my-4">

                                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn btn-dark btn-block btn-lg" OnClientClick="return validateShipping()" OnClick="btnConfirm_Click" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script type="text/javascript">
        function updateTotalPrice() {
            var shippingSelect = document.getElementById('<%= shippingSelect.ClientID %>');
            var totalAmount = document.getElementById('<%= lblTotalAmount.ClientID %>');
            var finalTotal = document.getElementById('<%= lblFinalTotal.ClientID %>');
            var couponCode = document.getElementById('<%= txtCouponCode.ClientID %>').value.trim().toUpperCase();

            if (!shippingSelect || !totalAmount || !finalTotal) return;

            var shippingCost = parseFloat(shippingSelect.value);
            var basePrice = parseFloat(totalAmount.innerText.replace(/[^0-9.-]+/g, ''));
            
            // Valid coupon codes and their discount amounts
            var validCoupons = {
                'SAVE5': 5,
                'DISCOUNT10': 10
            };

            // Get discount amount if coupon is valid
            var discountAmount = validCoupons[couponCode] || 0;
            
            // Calculate new total with both shipping and discount
            var newTotal = basePrice + shippingCost - discountAmount;
            
            console.log('Calculating total:');
            console.log('Base price:', basePrice);
            console.log('Shipping cost:', shippingCost);
            console.log('Discount amount:', discountAmount);
            console.log('Final total:', newTotal);
            
            finalTotal.innerText = newTotal.toFixed(0) + '€';
        }

        function validateShipping() {
            var shippingSelect = document.getElementById('<%= shippingSelect.ClientID %>');
            var shippingError = document.getElementById('shippingError');
            
            if (shippingSelect.value === "0") {
                shippingError.style.display = "block";
                return false;
            }
            
            shippingError.style.display = "none";
            return true;
        }

        // Add event listeners
        $(document).ready(function() {
            console.log('Document ready, calling updateTotalPrice');
            updateTotalPrice();

            // Add change event listener for shipping select
            $('#<%= shippingSelect.ClientID %>').on('change', function() {
                console.log('Shipping option changed');
                updateTotalPrice();
            });
        });

        // Add event listener for coupon code input
        document.getElementById('<%= txtCouponCode.ClientID %>').addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                document.getElementById('<%= btnApplyCoupon.ClientID %>').click();
            }
        });

        // Add event listener for UpdatePanel postback
        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, args) {
                console.log('UpdatePanel postback completed');
                updateTotalPrice();
            });
        }
    </script>
</asp:Content>
