using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Shopping.DataAccess1;
using Shopping.Model1;

namespace Shopping.Controller1
{
    public class ProductController
    {
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM \"TUNG\".\"PRODUCTS\"";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                ProductName = reader["PRODUCTNAME"].ToString(),
                                Description = reader["DESCRIPTION"] != DBNull.Value ? HttpUtility.HtmlDecode(reader["DESCRIPTION"].ToString()) : null,
                                QuantityPerUnit = reader["QUANTITYPERUNIT"] != DBNull.Value ? reader["QUANTITYPERUNIT"].ToString() : null,
                                UnitPrice = reader["UNITPRICE"] != DBNull.Value ? Convert.ToDecimal(reader["UNITPRICE"]) : (decimal?)null,
                                UnitsInStock = reader["UNITSINSTOCK"] != DBNull.Value ? Convert.ToInt32(reader["UNITSINSTOCK"]) : (int?)null,
                                UnitsOnOrder = reader["UNITSONORDER"] != DBNull.Value ? Convert.ToInt32(reader["UNITSONORDER"]) : (int?)null
                            };

                            // Load product images
                            product.ProductImages = GetProductImages(product.ProductID);
                            
                            products.Add(product);
                        }
                    }
                    return products;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error getting products: {ex.Message}");
                    return new List<Product>(); // Return empty list instead of throwing
                }
            }
        }

        public bool AddProduct(Product product)
        {
            if (product.ProductID <= 0)
            {
                throw new Exception("ProductID is required and must be greater than 0");
            }

            string query = @"INSERT INTO ""TUNG"".""PRODUCTS"" 
                (PRODUCTID, PRODUCTNAME, DESCRIPTION, QUANTITYPERUNIT, UNITPRICE, UNITSINSTOCK, UNITSONORDER) 
                VALUES (:ProductID, :ProductName, :Description, :QuantityPerUnit, :UnitPrice, :UnitsInStock, :UnitsOnOrder)";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = product.ProductID;
                        cmd.Parameters.Add("ProductName", OracleDbType.Varchar2).Value = product.ProductName ?? (object)DBNull.Value;
                        cmd.Parameters.Add("Description", OracleDbType.Clob).Value = !string.IsNullOrEmpty(product.Description) ? HttpUtility.HtmlEncode(product.Description) : (object)DBNull.Value;
                        cmd.Parameters.Add("QuantityPerUnit", OracleDbType.Varchar2).Value = product.QuantityPerUnit ?? (object)DBNull.Value;
                        cmd.Parameters.Add("UnitPrice", OracleDbType.Decimal).Value = product.UnitPrice.HasValue ? (object)product.UnitPrice.Value : DBNull.Value;
                        cmd.Parameters.Add("UnitsInStock", OracleDbType.Int32).Value = product.UnitsInStock.HasValue ? (object)product.UnitsInStock.Value : DBNull.Value;
                        cmd.Parameters.Add("UnitsOnOrder", OracleDbType.Int32).Value = product.UnitsOnOrder.HasValue ? (object)product.UnitsOnOrder.Value : DBNull.Value;

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error adding product: " + ex.Message);
                }
            }
        }

        public bool UpdateProduct(Product product)
        {
            if (product.ProductID <= 0)
            {
                throw new Exception("ProductID is required and must be greater than 0");
            }

            string query = @"UPDATE ""TUNG"".""PRODUCTS"" SET 
                PRODUCTNAME = :ProductName,
                DESCRIPTION = :Description,
                QUANTITYPERUNIT = :QuantityPerUnit,
                UNITPRICE = :UnitPrice,
                UNITSINSTOCK = :UnitsInStock,
                UNITSONORDER = :UnitsOnOrder
                WHERE PRODUCTID = :ProductID";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductName", OracleDbType.Varchar2).Value = product.ProductName ?? (object)DBNull.Value;
                        cmd.Parameters.Add("Description", OracleDbType.Clob).Value = !string.IsNullOrEmpty(product.Description) ? HttpUtility.HtmlEncode(product.Description) : (object)DBNull.Value;
                        cmd.Parameters.Add("QuantityPerUnit", OracleDbType.Varchar2).Value = product.QuantityPerUnit ?? (object)DBNull.Value;
                        cmd.Parameters.Add("UnitPrice", OracleDbType.Decimal).Value = product.UnitPrice.HasValue ? (object)product.UnitPrice.Value : DBNull.Value;
                        cmd.Parameters.Add("UnitsInStock", OracleDbType.Int32).Value = product.UnitsInStock.HasValue ? (object)product.UnitsInStock.Value : DBNull.Value;
                        cmd.Parameters.Add("UnitsOnOrder", OracleDbType.Int32).Value = product.UnitsOnOrder.HasValue ? (object)product.UnitsOnOrder.Value : DBNull.Value;
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = product.ProductID;

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating product: " + ex.Message);
                }
            }
        }

        public bool DeleteProduct(int productId)
        {
            string query = "DELETE FROM \"TUNG\".\"PRODUCTS\" WHERE PRODUCTID = :ProductID";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = productId;

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting product: " + ex.Message);
                }
            }
        }

        public List<ProductImage> GetProductImages(int productId)
        {
            List<ProductImage> images = new List<ProductImage>();
            string query = @"SELECT * FROM ""TUNG"".""PRODUCT_IMAGES"" WHERE PRODUCTID = :ProductID";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = productId;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductImage image = new ProductImage
                                {
                                    ProductImageID = Convert.ToInt32(reader["PRODUCTIMAGEID"]),
                                    ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                    ImageUrl = reader["IMAGEURL"].ToString(),
                                    OriginalFileName = reader["ORIGINALFILENAME"].ToString(),
                                    FileSize = Convert.ToInt64(reader["FILESIZE"]),
                                    ImageWidth = Convert.ToInt32(reader["IMAGEWIDTH"]),
                                    ImageHeight = Convert.ToInt32(reader["IMAGEHEIGHT"])
                                };
                                images.Add(image);
                            }
                        }
                    }
                    return images;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting product images: " + ex.Message);
                }
            }
        }

        public bool AddProductImage(int productId, HttpPostedFile imageFile)
        {
            // Validate the image
            var validationResult = ImageHelper.ValidateImage(imageFile);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ErrorMessage);
            }

            // Save the image to disk
            string fileName = ImageHelper.SaveImage(imageFile, productId.ToString());

            string query = @"INSERT INTO ""TUNG"".""PRODUCT_IMAGES"" 
                (PRODUCTIMAGEID, PRODUCTID, IMAGEURL, ORIGINALFILENAME, FILESIZE, IMAGEWIDTH, IMAGEHEIGHT) 
                VALUES (""TUNG"".""PRODUCT_IMAGES_SEQ"".NEXTVAL, :ProductID, :ImageUrl, :OriginalFileName, :FileSize, :ImageWidth, :ImageHeight)";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = productId;
                        cmd.Parameters.Add("ImageUrl", OracleDbType.Varchar2).Value = fileName;
                        cmd.Parameters.Add("OriginalFileName", OracleDbType.Varchar2).Value = imageFile.FileName;
                        cmd.Parameters.Add("FileSize", OracleDbType.Int64).Value = imageFile.ContentLength;
                        cmd.Parameters.Add("ImageWidth", OracleDbType.Int32).Value = validationResult.ImageWidth;
                        cmd.Parameters.Add("ImageHeight", OracleDbType.Int32).Value = validationResult.ImageHeight;

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error adding product image: " + ex.Message);
                }
            }
        }

        public bool DeleteProductImage(int productImageId)
        {
            // First get the image details to delete the file
            string getImageQuery = @"SELECT IMAGEURL FROM ""TUNG"".""PRODUCT_IMAGES"" WHERE PRODUCTIMAGEID = :ProductImageID";
            string imageUrl = null;

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    // Get image URL
                    using (OracleCommand cmd = new OracleCommand(getImageQuery, conn))
                    {
                        cmd.Parameters.Add("ProductImageID", OracleDbType.Int32).Value = productImageId;
                        imageUrl = cmd.ExecuteScalar()?.ToString();
                    }

                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        return false;
                    }

                    // Delete from database
                    string deleteQuery = @"DELETE FROM ""TUNG"".""PRODUCT_IMAGES"" WHERE PRODUCTIMAGEID = :ProductImageID";
                    using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.Add("ProductImageID", OracleDbType.Int32).Value = productImageId;
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            // Delete physical file
                            string imagePath = System.Web.HttpContext.Current.Server.MapPath("~/Image/" + imageUrl);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            return true;
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting product image: " + ex.Message);
                }
            }
        }

        public Product GetProductById(int productId)
        {
            Product product = null;
            string query = "SELECT * FROM \"TUNG\".\"PRODUCTS\" WHERE PRODUCTID = :ProductID";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = productId;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product = new Product
                                {
                                    ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    Description = reader["DESCRIPTION"] != DBNull.Value ? reader["DESCRIPTION"].ToString() : null,
                                    QuantityPerUnit = reader["QUANTITYPERUNIT"] != DBNull.Value ? reader["QUANTITYPERUNIT"].ToString() : null,
                                    UnitPrice = reader["UNITPRICE"] != DBNull.Value ? Convert.ToDecimal(reader["UNITPRICE"]) : (decimal?)null,
                                    UnitsInStock = reader["UNITSINSTOCK"] != DBNull.Value ? Convert.ToInt32(reader["UNITSINSTOCK"]) : (int?)null,
                                    UnitsOnOrder = reader["UNITSONORDER"] != DBNull.Value ? Convert.ToInt32(reader["UNITSONORDER"]) : (int?)null,
                                    ProductImages = new List<ProductImage>()
                                };

                                // Get images for this product
                                product.ProductImages = GetProductImages(productId);
                            }
                        }
                    }
                    return product;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting product: " + ex.Message);
                }
            }
        }
    }
} 