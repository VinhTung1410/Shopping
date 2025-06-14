<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="Shopping.View.ProductDetails" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    <div class="container py-4">
        <div class="row">
            <!-- Ảnh sản phẩm & slider -->
            <div class="col-md-6">
                <div class="main-image mb-3 text-center">
                    <asp:Image ID="imgMain" runat="server" CssClass="img-fluid rounded shadow" Style="max-height:400px;" />
                </div>
                <div class="thumb-slider d-flex gap-2 justify-content-center">
                    <asp:Repeater ID="ImagesRepeater" runat="server">
                        <ItemTemplate>
                            <img src='<%# ResolveUrl("~/Image/" + Eval("ImageUrl")) %>' alt="Thumb" class="img-thumbnail" style="width:60px; height:60px; cursor:pointer;" onclick="document.getElementById('<%= imgMain.ClientID %>').src=this.src;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <!-- Thông tin sản phẩm -->
            <div class="col-md-6">
                <h1 class="fw-bold mb-2" style="font-size:2rem;">
                    <asp:Label ID="lblProductName" runat="server" />
                </h1>
                <div class="mb-2">
                    <span class="text-warning">
                        <i class="fas fa-star"></i> 4.8
                    </span>
                    <span class="text-muted ms-2">(346 feedback)</span>
                </div>
                <div class="mb-3">
                    <span class="text-decoration-line-through text-muted" style="font-size:1.1rem;">
                        <asp:Label ID="lblOriginalPrice" runat="server" />
                    </span>
                    <span class="fw-bold" style="color:#ff5722; font-size:1.7rem;">
                        <asp:Label ID="lblUnitPrice" runat="server" />$
                    </span>
                    <span class="badge bg-danger ms-2">-<asp:Label ID="lblDiscount" runat="server" />20%</span>
                </div>
                <div class="mb-3 d-flex align-items-center">
                    <b class="me-2">Quantity:</b>
                    <div class="input-group" style="width:auto;max-width:180px;">
                        <button type="button" class="btn btn-outline-secondary" onclick="decreaseQty()">-</button>
                        <asp:TextBox ID="txtQuantity" runat="server" Text="1" CssClass="form-control text-center" style="width:60px;" TextMode="Number" />
                        <button type="button" class="btn btn-outline-secondary" onclick="increaseQty()">+</button>
                    </div>
                    <span id="qtyError" class="ms-3 text-danger"></span>
                    <span class="ms-3 text-muted"><asp:Label ID="lblUnitsInStock" runat="server" /> products left</span>
                </div>
                <div id="outOfStockMessage" class="mb-3 text-danger fw-bold" style="display:none;">Temporarily out of stock</div>
                <div class="d-flex gap-2 mb-3">
                    <asp:Button ID="btnBuyNow" runat="server" Text="Buy now" CssClass="btn btn-warning fw-bold px-4" OnClick="btnBuyNow_Click"/>
                    <asp:Button ID="btnAddToCart" runat="server" Text="Add to cart" OnClick="btnAddToCart_Click" CssClass="btn btn-danger fw-bold px-4" />
                </div>
                <div class="mb-3">
                    <i class="fas fa-truck"></i> Ship: <span class="text-success">3-6/6</span> &nbsp;|&nbsp;
                    <span class="text-success">Freeship</span> &nbsp;|&nbsp;
                    <span class="text-primary">Voucher </span>
                </div>
                <div class="mb-3">
                    <i class="fas fa-undo"></i> 15 days free return &nbsp;|&nbsp;
                    <i class="fas fa-shield-alt"></i> Product insurance
                </div>
                <div class="mb-3">
                    <b>Share:</b>
                    <a href="#" class="ms-2 text-primary"><i class="fab fa-facebook"></i></a>
                    <a href="#" class="ms-2 text-info"><i class="fab fa-facebook-messenger"></i></a>
                    <a href="#" class="ms-2 text-danger"><i class="fab fa-pinterest"></i></a>
                </div>
                <div class="mb-3">
                    <b>Description:</b>
                    <div class="text-secondary"><asp:Label ID="lblDescription" runat="server" /></div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function getUnitInStock() {
            var lblStockElement = document.getElementById('<%= lblUnitsInStock.ClientID %>');
            var stock = 0;
            if (lblStockElement) {
                stock = parseInt(lblStockElement.innerText.trim()); // .trim() để loại bỏ khoảng trắng thừa
            }
            console.log('Stock from lblUnitsInStock:', lblStockElement ? lblStockElement.innerText.trim() : 'Element not found', 'Parsed Stock:', stock);
            return isNaN(stock) ? 0 : stock;
        }

        function decreaseQty() {
            var qtyInput = document.getElementById('<%= txtQuantity.ClientID %>');
            var qty = parseInt(qtyInput.value) || 1;
            if (qty > 1) {
                qtyInput.value = qty - 1;
                document.getElementById('qtyError').innerText = '';
            }
            updateButtonStates(); // Call this to re-evaluate button states
            console.log('After decrease - Quantity:', qtyInput.value);
        }

        function increaseQty() {
            var qtyInput = document.getElementById('<%= txtQuantity.ClientID %>');
            var qty = parseInt(qtyInput.value) || 1;
            var maxStock = getUnitInStock();
            
            console.log('Before increase - Current Qty:', qty, 'Max Stock:', maxStock);

            if (qty < maxStock) {
                qtyInput.value = qty + 1;
                document.getElementById('qtyError').innerText = '';
            } else {
                qtyInput.value = maxStock; // Đảm bảo không vượt quá maxStock
                document.getElementById('qtyError').innerText = 'Exceeded the available inventory!';
            }
            updateButtonStates(); // Call this to re-evaluate button states
            console.log('After increase - Quantity:', qtyInput.value, 'Error:', document.getElementById('qtyError').innerText);
        }

        function updateButtonStates() {
            var maxStock = getUnitInStock();
            var btnBuyNow = document.getElementById('<%= btnBuyNow.ClientID %>');
            var btnAddToCart = document.getElementById('<%= btnAddToCart.ClientID %>');
            var outOfStockMessage = document.getElementById('outOfStockMessage');
            var qtyInput = document.getElementById('<%= txtQuantity.ClientID %>');
            var decreaseButton = qtyInput.previousElementSibling;
            var increaseButton = qtyInput.nextElementSibling;


            if (maxStock === 0) {
                btnBuyNow.disabled = true;
                btnAddToCart.disabled = true;
                btnBuyNow.classList.add('btn-secondary');
                btnBuyNow.classList.remove('btn-warning');
                btnAddToCart.classList.add('btn-secondary');
                btnAddToCart.classList.remove('btn-danger');
                outOfStockMessage.style.display = 'block';
                qtyInput.disabled = true;
                decreaseButton.disabled = true;
                increaseButton.disabled = true;
                qtyInput.value = 0; // Set quantity to 0 if out of stock
            } else {
                btnBuyNow.disabled = false;
                btnAddToCart.disabled = false;
                btnBuyNow.classList.remove('btn-secondary');
                btnBuyNow.classList.add('btn-warning');
                btnAddToCart.classList.remove('btn-secondary');
                btnAddToCart.classList.add('btn-danger');
                outOfStockMessage.style.display = 'none';
                qtyInput.disabled = false;
                decreaseButton.disabled = false;
                increaseButton.disabled = false;
                if (parseInt(qtyInput.value) === 0) {
                    qtyInput.value = 1; // Reset quantity to 1 if it was 0 and now in stock
                }
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            var qtyInput = document.getElementById('<%= txtQuantity.ClientID %>');
            qtyInput.addEventListener('input', function () {
                var qty = parseInt(qtyInput.value) || 1;
                var maxStock = getUnitInStock();
                
                console.log('Input event - Current Qty:', qty, 'Max Stock:', maxStock);

                if (qty > maxStock) {
                    qtyInput.value = maxStock;
                    document.getElementById('qtyError').innerText = 'Exceeded the available inventory!';
                } else {
                    document.getElementById('qtyError').innerText = '';
                }
                if (qty < 1) {
                    qtyInput.value = 1;
                }
                updateButtonStates(); // Call this to re-evaluate button states
                console.log('Input event - Final Qty:', qtyInput.value, 'Error:', document.getElementById('qtyError').innerText);
            });
            updateButtonStates(); // Initial call to set button states on page load
        });
    </script>
</asp:Content>
