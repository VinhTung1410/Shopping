using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Shopping
{
    public partial class _Default : Page
    {
        //    protected void Page_Load(object sender, EventArgs e)
        //    {
        //        if (!IsPostBack)
        //        {
        //            BindFeaturedMovies();
        //            BindLatestMovies();
        //            BindProductGrid();
        //        }
        //    }

        //    private void BindFeaturedMovies()
        //    {
        //        var movies = new List<Movie>
        //        {
        //            new Movie { Title = "Phim Đề Cử 1", Year = "2024", Rating = "8.5" },
        //            new Movie { Title = "Phim Đề Cử 2", Year = "2024", Rating = "8.7" },
        //            new Movie { Title = "Phim Đề Cử 3", Year = "2024", Rating = "8.3" },
        //            new Movie { Title = "Phim Đề Cử 4", Year = "2024", Rating = "8.9" },
        //            new Movie { Title = "Phim Đề Cử 5", Year = "2024", Rating = "8.6" },
        //            new Movie { Title = "Phim Đề Cử 6", Year = "2024", Rating = "8.4" }
        //        };

        //    }

        //    private void BindLatestMovies()
        //    {
        //        var movies = new List<Movie>();
        //        for (int i = 1; i <= 12; i++)
        //        {
        //            movies.Add(new Movie
        //            {
        //                Title = $"Phim Mới {i}",
        //                Year = "2024",
        //                Rating = ((7.5 + (i * 0.1)) % 10).ToString("F1")
        //            });
        //        }
        //    }

        //    private void BindProductGrid()
        //    {
        //        var products = new List<Product>();

        //        // Add sample products
        //        for (int i = 1; i <= 32; i++)
        //        {
        //            var originalPrice = i * 100000 + 1500000;
        //            var discount = (i % 3 == 0) ? 40 : 0;
        //            var currentPrice = originalPrice * (100 - discount) / 100;

        //            products.Add(new Product
        //            {
        //                Name = $"Sản phẩm thể thao {i}",
        //                ImageUrl = $"https://placehold.co/300x400",
        //                HoverImageUrl = $"https://placehold.co/300x400/666/fff",
        //                CurrentPrice = currentPrice,
        //                OriginalPrice = originalPrice,
        //                IsNew = i <= 8,
        //                Discount = discount,
        //                HasPromotion = i % 4 == 0,
        //                ColorOptions = new List<string> { "#000000", "#ff0000", "#0000ff" }
        //            });
        //        }

        //        //ProductGridRepeater.DataSource = products;
        //        //ProductGridRepeater.DataBind();
        //    }

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
        //}

        //public class Movie
        //{
        //    public string Title { get; set; }
        //    public string Year { get; set; }
        //    public string Rating { get; set; }
        //}

        //public class Product
        //{
        //    public string Name { get; set; }
        //    public string ImageUrl { get; set; }
        //    public string HoverImageUrl { get; set; }
        //    public decimal CurrentPrice { get; set; }
        //    public decimal OriginalPrice { get; set; }
        //    public bool IsNew { get; set; }
        //    public int Discount { get; set; }
        //    public bool HasPromotion { get; set; }
        //    public List<string> ColorOptions { get; set; }
    }
}