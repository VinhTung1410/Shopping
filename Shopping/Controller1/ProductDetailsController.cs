using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Shopping.DataAccess1;
using Shopping.Model1;
using System.Web;
using System.Configuration;

namespace Shopping.Controller1
{
    public class ProductDetailsController
    {
        private readonly string connectionString;

        public ProductDetailsController()
        {
            var connStr = ConfigurationManager.ConnectionStrings["OracleConn"];
            if (connStr == null)
            {
                throw new ConfigurationErrorsException("Connection string 'OracleConn' not found in Web.config");
            }
            connectionString = connStr.ConnectionString;
        }

        public Product GetProductDetails(int productId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                        SELECT p.*, 
                               (SELECT COUNT(*) FROM TUNG.PRODUCT_IMAGES WHERE PRODUCTID = p.PRODUCTID) as ImageCount
                        FROM TUNG.PRODUCTS p
                        WHERE p.PRODUCTID = :ProductID";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;
                        conn.Open();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    Description = reader["DESCRIPTION"] != DBNull.Value ? 
                                        HttpUtility.HtmlDecode(reader["DESCRIPTION"].ToString()) : null,
                                    QuantityPerUnit = reader["QUANTITYPERUNIT"] != DBNull.Value ? 
                                        reader["QUANTITYPERUNIT"].ToString() : null,
                                    UnitPrice = reader["UNITPRICE"] != DBNull.Value ? 
                                        Convert.ToDecimal(reader["UNITPRICE"]) : (decimal?)null,
                                    UnitsInStock = reader["UNITSINSTOCK"] != DBNull.Value ? 
                                        Convert.ToInt32(reader["UNITSINSTOCK"]) : (int?)null,
                                    UnitsOnOrder = reader["UNITSONORDER"] != DBNull.Value ? 
                                        Convert.ToInt32(reader["UNITSONORDER"]) : (int?)null,
                                    ProductImages = GetProductImages(productId)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetProductDetails: {ex.Message}");
                throw;
            }
            return null;
        }

        public List<ProductImage> GetProductImages(int productId)
        {
            List<ProductImage> images = new List<ProductImage>();
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"SELECT * FROM TUNG.PRODUCT_IMAGES WHERE PRODUCTID = :ProductID";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                images.Add(new ProductImage
                                {
                                    ProductImageID = Convert.ToInt32(reader["PRODUCTIMAGEID"]),
                                    ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                    ImageUrl = reader["IMAGEURL"].ToString(),
                                    OriginalFileName = reader["ORIGINALFILENAME"].ToString(),
                                    FileSize = reader["FILESIZE"] != DBNull.Value ? Convert.ToInt64(reader["FILESIZE"]) : 0,
                                    ImageWidth = reader["IMAGEWIDTH"] != DBNull.Value ? Convert.ToInt32(reader["IMAGEWIDTH"]) : 0,
                                    ImageHeight = reader["IMAGEHEIGHT"] != DBNull.Value ? Convert.ToInt32(reader["IMAGEHEIGHT"]) : 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetProductImages: {ex.Message}");
                throw;
            }
            return images;
        }
    }
}

