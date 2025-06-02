using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Shopping.Controller1;
using Shopping.Model1;
using System.IO;

namespace Shopping
{
    public partial class _Default : Page
    {
        private ProductController productController;

        protected void Page_Load(object sender, EventArgs e)
        {
            productController = new ProductController();

            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = productController.GetAllProducts();

                // Load featured products - take first 10 products
                FeaturedProductsRepeater.DataSource = products.Take(10);
                FeaturedProductsRepeater.DataBind();

                // Load all products for grid
                ProductGridRepeater.DataSource = products;
                ProductGridRepeater.DataBind();
            }
            catch (Exception ex)
            {
                // Log the error or show a user-friendly message
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }
        }

        protected string GetFirstProductImage(object dataItem)
        {
            try
            {
                var product = dataItem as Product;
                if (product != null && product.ProductImages != null && product.ProductImages.Any())
                {
                    var firstImage = product.ProductImages.First();
                    string imagePath = firstImage.ImageUrl;
                    
                    // Check if the image file exists
                    string physicalPath = Server.MapPath("~/Image/" + imagePath);
                    if (File.Exists(physicalPath))
                    {
                        return ResolveUrl("~/Image/" + imagePath);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Image file not found: {physicalPath}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"No images found for product ID: {(product != null ? product.ProductID.ToString() : "null")}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting product image: {ex.Message}");
            }

            // Return a default placeholder image if no image is found or an error occurs
            return ResolveUrl("~/Content/images/no-image.jpg");
        }

        protected string GetColorOptionsHtml(List<string> colors)
        {
            if (colors == null || colors.Count == 0)
                return string.Empty;

            var html = "";
            foreach (var color in colors)
            {
                html += $"<span class=\"color-option\" style=\"background-color: {color}\" title=\"Màu {color}\"></span>";
            }
            return html;
        }
    }
}