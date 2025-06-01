using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Model1
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? UnitsInStock { get; set; }

        public int? UnitsOnOrder { get; set; }

        // Collection of product images
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        // Additional display properties
        public bool IsNew 
        { 
            get 
            {
                // Consider a product new if it was added within the last 30 days
                // You can modify this logic based on your business rules
                return true; // Temporary return true for testing
            } 
        }

        public int Discount
        {
            get
            {
                // You can implement your discount calculation logic here
                return 0; // Temporary return 0 for testing
            }
        }

        public decimal OriginalPrice
        {
            get
            {
                // Return UnitPrice if no discount, otherwise calculate original price
                return UnitPrice.HasValue ? UnitPrice.Value : 0;
            }
        }

        public bool HasPromotion
        {
            get
            {
                // Implement your promotion logic here
                return false; // Temporary return false for testing
            }
        }

        public Product()
        {
            ProductImages = new List<ProductImage>();
        }
    }

    public class ProductImage
    {
        public int ProductImageID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        // Store the original filename
        public string OriginalFileName { get; set; }

        // Store file size in bytes
        public long FileSize { get; set; }

        // Store image dimensions
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        // Navigation property
        public virtual Product Product { get; set; }

        // Validation constants
        public const long MaxFileSizeBytes = 25 * 1024 * 1024; // 25MB
        public const int MaxImageWidth = 1500;
        public const int MaxImageHeight = 1500;
        public const string AllowedExtension = ".jpg";
    }
}