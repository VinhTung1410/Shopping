<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Shopping._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Font Awesome CDN -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    
    <div class="container-fluid px-0">
        <!-- Featured Products Section -->
        <section class="featured-section py-4">
            <div class="container">
                <div class="section-header d-flex justify-content-between align-items-center mb-4">
                    <h2 class="section-title">Sản Phẩm Nổi Bật</h2>
                    <div class="slider-nav">
                        <button type="button" class="nav-btn prev" onclick="moveSlider('left')">
                            <i class="fas fa-chevron-left"></i>
                        </button>
                        <button type="button" class="nav-btn next" onclick="moveSlider('right')">
                            <i class="fas fa-chevron-right"></i>
                        </button>
                    </div>
                </div>
                
                <div class="featured-slider">
                    <div class="slider-container">
                        <div class="slider-track">
                            <asp:Repeater ID="FeaturedProductsRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="product-card">
                                        <div class="product-image">
                                            <a href='<%# ResolveUrl("~/View/ProductDetails.aspx?productId=" + Eval("ProductID")) %>'>
                                                <img src='<%# GetFirstProductImage(Container.DataItem) %>' 
                                                     alt='<%# Eval("ProductName") %>'
                                                     onerror="this.onerror=null; this.src='Content/images/no-image.jpg';"
                                                     class="img-fluid product-img" />
                                            </a>
                                            <div class="product-overlay">
                                                <div class="overlay-content">
                                                    <span class="status">New</span>
                                                    <div class="action-buttons">
                                                        <button class="btn-action"><i class="fas fa-heart"></i></button>
                                                        <a href='<%# ResolveUrl("~/View/ProductDetails.aspx?productId=" + Eval("ProductID")) %>' class="btn-action"><i class="fas fa-shopping-cart"></i></a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="product-info">
                                            <h3 class="product-title">
                                                <a href='<%# ResolveUrl("~/View/ProductDetails.aspx?productId=" + Eval("ProductID")) %>'><%# Eval("ProductName") %></a>
                                            </h3>
                                            <div class="product-meta">
                                                <span class="price"><%# String.Format("{0:N0}", Eval("UnitPrice")) %></span>
                                                <span class="unit"><%# Eval("QuantityPerUnit") %></span>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <!-- Product Grid Section -->
    <section class="product-grid-section py-5">
        <div class="container">
            <div class="section-header d-flex justify-content-between align-items-center mb-4">
                <h2 class="section-title">Sản Phẩm Mới</h2>
                <div class="view-options">
                    <select class="form-select sort-select">
                        <option>Sắp xếp theo</option>
                        <option>Giá: Thấp đến Cao</option>
                        <option>Giá: Cao đến Thấp</option>
                        <option>Mới nhất</option>
                    </select>
                </div>
            </div>

            <div class="row g-4">
                <asp:Repeater ID="ProductGridRepeater" runat="server">
                    <ItemTemplate>
                        <div class="col-6 col-md-4 col-lg-3">
                            <div class="product-card h-100">
                                <div class="product-image">
                                    <a href='<%# ResolveUrl("~/View/ProductDetails.aspx?productId=" + Eval("ProductID")) %>' class="product-link">
                                        <div class="image-container">
                                            <img src='<%# GetFirstProductImage(Container.DataItem) %>' 
                                                 alt='<%# Eval("ProductName") %>'
                                                 onerror="this.onerror=null; this.src='Content/images/no-image.jpg';"
                                                 class="main-image product-img" />
                                            <img src='<%# GetFirstProductImage(Container.DataItem) %>' 
                                                 alt='<%# Eval("ProductName") %>'
                                                 onerror="this.onerror=null; this.src='Content/images/no-image.jpg';"
                                                 class="hover-image product-img" />
                                        </div>
                                        <%# Eval("IsNew").ToString().ToLower() == "true" ? "<span class=\"badge-new\">New</span>" : "" %>
                                        <%# Convert.ToInt32(Eval("Discount")) > 0 ? "<span class=\"badge-discount\">-" + Eval("Discount") + "%</span>" : "" %>
                                    </a>
                                    <div class="product-actions">
                                        <button class="btn-wishlist" title="Thêm vào yêu thích">
                                            <i class="far fa-heart"></i>
                                        </button>
                                        <a href='<%# ResolveUrl("~/View/ProductDetails.aspx?productId=" + Eval("ProductID")) %>' 
                                           class="btn-quickview" 
                                           title="Xem nhanh">
                                            <i class="far fa-eye"></i>
                                        </a>
                                    </div>
                                </div>
                                <div class="product-info">
                                    <h3 class="product-title">
                                        <a href='<%# ResolveUrl("~/View/ProductDetails.aspx?productId=" + Eval("ProductID")) %>'><%# Eval("ProductName") %></a>
                                    </h3>
                                    <div class="product-meta">
                                        <span class="unit"><%# Eval("QuantityPerUnit") %></span>
                                    </div>
                                    <div class="product-price">
                                        <span class="current-price"><%# String.Format("{0:N0}", Eval("UnitPrice")) %></span>
                                        <%# Convert.ToDecimal(Eval("OriginalPrice")) != Convert.ToDecimal(Eval("UnitPrice")) ? 
                                            "<span class=\"original-price\">" + String.Format("{0:N0}", Eval("OriginalPrice")) + "</span>" : "" %>
                                    </div>
                                    <%# Convert.ToBoolean(Eval("HasPromotion")) ? 
                                        "<div class=\"promotion-tag\">Áp dụng Giảm Thêm 40%*</div>" : "" %>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="text-center mt-5">
                <button class="btn btn-outline-dark btn-load-more">Xem thêm sản phẩm</button>
            </div>
        </div>
    </section>

    <style>
        /* General Styles */
        body {
            background-color: #f8f9fa;
            color: #333;
            font-family: 'Roboto', sans-serif;
        }

        .container {
            max-width: 1200px;
        }

        /* Section Styles */
        .section-title {
            font-size: 1.5rem;
            font-weight: 600;
            margin: 0;
            color: #333;
        }

        .slider-nav {
            display: flex;
            gap: 0.5rem;
        }

        .nav-btn {
            width: 32px;
            height: 32px;
            border: none;
            border-radius: 50%;
            background-color: #fff;
            color: #333;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
        }

        .nav-btn:hover {
            background-color: #333;
            color: #fff;
        }

        /* Featured Slider Styles */
        .featured-slider {
            position: relative;
            margin: 0 -8px;
        }

        .slider-container {
            overflow: hidden;
            padding: 8px 0;
        }

        .slider-track {
            display: flex;
            transition: transform 0.5s ease;
            gap: 16px;
            padding: 0 8px;
        }

        /* Product Card Styles */
        .product-card {
            flex: 0 0 200px;
            background: #fff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            transition: transform 0.3s ease;
        }

        .product-card:hover {
            transform: translateY(-5px);
        }

        .product-image {
            position: relative;
            padding-top: 133%; /* 4:3 Aspect Ratio */
            overflow: hidden;
        }

        .product-image img {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: transform 0.3s ease;
        }

        .product-card:hover .product-image img {
            transform: scale(1.1);
        }

        .product-overlay {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0,0,0,0.5);
            opacity: 0;
            transition: opacity 0.3s ease;
            display: flex;
            align-items: flex-end;
        }

        .product-card:hover .product-overlay {
            opacity: 1;
        }

        .overlay-content {
            width: 100%;
            padding: 1rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .status {
            background: #ff6b6b;
            color: #fff;
            padding: 0.25rem 0.75rem;
            border-radius: 4px;
            font-size: 0.875rem;
        }

        .action-buttons {
            display: flex;
            gap: 0.5rem;
        }

        .btn-action {
            width: 32px;
            height: 32px;
            border: none;
            border-radius: 50%;
            background: #fff;
            color: #333;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .btn-action:hover {
            background: #333;
            color: #fff;
        }

        .product-info {
            padding: 1rem;
        }

        .product-title {
            font-size: 1rem;
            margin: 0 0 0.5rem;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .product-meta {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 0.5rem;
            font-size: 0.875rem;
        }

        .unit {
            color: #666;
            font-size: 0.875rem;
        }

        .price, .current-price {
            color: #ff6b6b;
            font-weight: 600;
            font-size: 1rem;
            position: relative;
        }

        .price::before, 
        .current-price::before {
            content: "$";
            font-size: 0.8em;
            position: relative;
            top: -0.1em;
            margin-right: 0.1em;
        }

        .original-price {
            text-decoration: line-through;
            color: #999;
            font-size: 0.875rem;
            margin-left: 0.5rem;
            position: relative;
        }

        .original-price::before {
            content: "$";
            font-size: 0.8em;
            position: relative;
            top: -0.1em;
            margin-right: 0.1em;
        }

        .rating {
            color: #ffd700;
        }

        .rating i {
            margin-right: 0.25rem;
        }

        @media (max-width: 768px) {
            .product-card {
                flex: 0 0 160px;
            }
        }

        /* Product Grid Styles */
        .product-grid-section {
            background-color: #fff;
        }

        .sort-select {
            padding: 0.5rem 2rem 0.5rem 1rem;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            background-color: #fff;
            font-size: 0.875rem;
        }

        .product-card {
            background: #fff;
            border: none;
            transition: all 0.3s ease;
            position: relative;
        }

        .product-card:hover {
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }

        .product-image {
            position: relative;
            padding-top: 133%;
            overflow: hidden;
        }

        .image-container {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
        }

        .main-image,
        .hover-image {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: opacity 0.3s ease;
        }

        .hover-image {
            opacity: 0;
        }

        .product-card:hover .hover-image {
            opacity: 1;
        }

        .badge-new,
        .badge-discount {
            position: absolute;
            top: 10px;
            left: 10px;
            padding: 0.25rem 0.75rem;
            font-size: 0.75rem;
            font-weight: 600;
            border-radius: 4px;
        }

        .badge-new {
            background-color: #000;
            color: #fff;
        }

        .badge-discount {
            background-color: #ff0000;
            color: #fff;
        }

        .product-actions {
            position: absolute;
            top: 10px;
            right: 10px;
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
            opacity: 0;
            transform: translateX(10px);
            transition: all 0.3s ease;
        }

        .product-card:hover .product-actions {
            opacity: 1;
            transform: translateX(0);
        }

        .btn-wishlist,
        .btn-quickview {
            width: 32px;
            height: 32px;
            border: none;
            border-radius: 50%;
            background: #fff;
            color: #333;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            text-decoration: none;
        }

        .btn-wishlist:hover,
        .btn-quickview:hover {
            background: #333;
            color: #fff;
            text-decoration: none;
        }

        .product-info {
            padding: 1rem;
        }

        .color-options {
            display: flex;
            gap: 0.5rem;
            margin-bottom: 0.5rem;
        }

        .color-option {
            width: 16px;
            height: 16px;
            border-radius: 50%;
            border: 1px solid #dee2e6;
        }

        .product-title {
            font-size: 0.875rem;
            margin-bottom: 0.5rem;
            font-weight: 400;
        }

        .product-title a {
            color: #333;
            text-decoration: none;
        }

        .product-price {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            margin-bottom: 0.25rem;
        }

        .promotion-tag {
            font-size: 0.75rem;
            color: #ff0000;
        }

        .btn-load-more {
            padding: 0.75rem 2rem;
            font-size: 0.875rem;
            border: 1px solid #333;
            background: transparent;
            color: #333;
            transition: all 0.3s ease;
        }

        .btn-load-more:hover {
            background: #333;
            color: #fff;
        }

        @media (max-width: 768px) {
            .product-title {
                font-size: 0.8125rem;
            }

            .current-price {
                font-size: 0.875rem;
            }
        }

        .product-img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: transform 0.3s ease;
        }
    </style>

    <script>
        let currentPosition = 0;
        const sliderTrack = document.querySelector('.slider-track');
        const cards = document.querySelectorAll('.featured-slider .product-card');
        let cardWidth = 216; // Initial card width + gap
        let maxPosition = 0;
        const maxVisibleProducts = 10; // Maximum number of products to show

        function updateSliderDimensions() {
            const container = document.querySelector('.slider-container');
            const containerWidth = container.offsetWidth;
            
            // Update card width based on screen size
            if (window.innerWidth <= 768) {
                cardWidth = 176; // Smaller cards on mobile (160px + 16px gap)
            } else {
                cardWidth = 216; // Original size on desktop (200px + 16px gap)
            }

            // Calculate total content width for exactly 10 products
            const totalContentWidth = Math.min(cards.length, maxVisibleProducts) * cardWidth;
            
            // Calculate new maxPosition
            maxPosition = -(totalContentWidth - containerWidth);
            
            // Ensure maxPosition is never positive
            maxPosition = Math.min(0, maxPosition);
            
            // Adjust current position if needed
            if (currentPosition < maxPosition) {
                currentPosition = maxPosition;
            }
            
            // Apply the new position
            sliderTrack.style.transform = `translateX(${currentPosition}px)`;
            
            // Update slider track width to fit exactly 10 products
            sliderTrack.style.width = `${totalContentWidth}px`;

            // Hide navigation buttons if not needed
            const prevButton = document.querySelector('.nav-btn.prev');
            const nextButton = document.querySelector('.nav-btn.next');
            
            if (containerWidth >= totalContentWidth) {
                prevButton.style.display = 'none';
                nextButton.style.display = 'none';
            } else {
                prevButton.style.display = 'flex';
                nextButton.style.display = 'flex';
            }
        }

        function moveSlider(direction) {
            const container = document.querySelector('.slider-container');
            const containerWidth = container.offsetWidth;
            const cardsToMove = Math.floor(containerWidth / cardWidth);
            const moveAmount = cardsToMove * cardWidth;

            if (direction === 'right' && currentPosition > maxPosition) {
                currentPosition = Math.max(maxPosition, currentPosition - moveAmount);
            } else if (direction === 'left' && currentPosition < 0) {
                currentPosition = Math.min(0, currentPosition + moveAmount);
            }

            sliderTrack.style.transform = `translateX(${currentPosition}px)`;

            // Update navigation buttons visibility
            const prevButton = document.querySelector('.nav-btn.prev');
            const nextButton = document.querySelector('.nav-btn.next');
            
            prevButton.style.opacity = currentPosition < 0 ? '1' : '0.5';
            nextButton.style.opacity = currentPosition > maxPosition ? '1' : '0.5';
        }

        // Initialize slider dimensions
        updateSliderDimensions();

        // Add resize event listener
        let resizeTimeout;
        window.addEventListener('resize', () => {
            // Debounce resize event
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                updateSliderDimensions();
            }, 250); // Wait for 250ms after last resize event
        });

        // Touch events for mobile
        let startX = null;
        let startPosition = null;

        sliderTrack.addEventListener('touchstart', (e) => {
            startX = e.touches[0].clientX;
            startPosition = currentPosition;
            sliderTrack.style.transition = 'none';
        });

        sliderTrack.addEventListener('touchmove', (e) => {
            if (!startX) return;
            
            const currentX = e.touches[0].clientX;
            const diff = currentX - startX;
            const newPosition = startPosition + diff;
            
            // Add boundaries to prevent over-scrolling
            if (newPosition <= 0 && newPosition >= maxPosition) {
                currentPosition = newPosition;
                sliderTrack.style.transform = `translateX(${currentPosition}px)`;
            }
        });

        sliderTrack.addEventListener('touchend', () => {
            startX = null;
            sliderTrack.style.transition = 'transform 0.5s ease';
            
            // Snap to nearest card position within bounds
            const cardPosition = Math.round(currentPosition / cardWidth) * cardWidth;
            currentPosition = Math.min(0, Math.max(maxPosition, cardPosition));
            sliderTrack.style.transform = `translateX(${currentPosition}px)`;
        });

        // Initialize button states
        document.addEventListener('DOMContentLoaded', () => {
            const prevButton = document.querySelector('.nav-btn.prev');
            const nextButton = document.querySelector('.nav-btn.next');
            
            prevButton.style.opacity = '0.5';
            nextButton.style.opacity = currentPosition > maxPosition ? '1' : '0.5';
        });
    </script>
</asp:Content>
