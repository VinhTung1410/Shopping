using System;
using System.Collections.Generic;
using System.Data;
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
                                QuantityPerUnit = reader["QUANTITYPERUNIT"] != DBNull.Value ? reader["QUANTITYPERUNIT"].ToString() : null,
                                UnitPrice = reader["UNITPRICE"] != DBNull.Value ? Convert.ToDecimal(reader["UNITPRICE"]) : (decimal?)null,
                                UnitsInStock = reader["UNITSINSTOCK"] != DBNull.Value ? Convert.ToInt32(reader["UNITSINSTOCK"]) : (int?)null,
                                UnitsOnOrder = reader["UNITSONORDER"] != DBNull.Value ? Convert.ToInt32(reader["UNITSONORDER"]) : (int?)null
                            };
                            products.Add(product);
                        }
                    }
                    return products;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting products: " + ex.Message);
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
                (PRODUCTID, PRODUCTNAME, QUANTITYPERUNIT, UNITPRICE, UNITSINSTOCK, UNITSONORDER) 
                VALUES (:ProductID, :ProductName, :QuantityPerUnit, :UnitPrice, :UnitsInStock, :UnitsOnOrder)";

            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("ProductID", OracleDbType.Int32).Value = product.ProductID;
                        cmd.Parameters.Add("ProductName", OracleDbType.Varchar2).Value = product.ProductName ?? (object)DBNull.Value;
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

        public Product GetProductById(int productId)
        {
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
                                return new Product
                                {
                                    ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    QuantityPerUnit = reader["QUANTITYPERUNIT"] != DBNull.Value ? reader["QUANTITYPERUNIT"].ToString() : null,
                                    UnitPrice = reader["UNITPRICE"] != DBNull.Value ? Convert.ToDecimal(reader["UNITPRICE"]) : (decimal?)null,
                                    UnitsInStock = reader["UNITSINSTOCK"] != DBNull.Value ? Convert.ToInt32(reader["UNITSINSTOCK"]) : (int?)null,
                                    UnitsOnOrder = reader["UNITSONORDER"] != DBNull.Value ? Convert.ToInt32(reader["UNITSONORDER"]) : (int?)null
                                };
                            }
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting product: " + ex.Message);
                }
            }
        }
    }
} 