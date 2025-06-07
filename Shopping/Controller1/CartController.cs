using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopping.Model1;
using Shopping.DataAccess1;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Shopping.Controller1
{
    public class CartController
    {
        public void AddToCart(int employeeId, int productId, int quantity)
        {
            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                OracleTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    // 1. Find or Create a pending Order for the employee
                    int orderId = GetOrCreatePendingOrder(employeeId, conn, transaction);

                    // 2. Get Product UnitPrice and Discount
                    decimal unitPrice = 0;
                    double discount = 0;
                    string getProductPriceQuery = "SELECT UNITPRICE FROM TUNG.PRODUCTS WHERE PRODUCTID = :p_productId";
                    using (OracleCommand getPriceCmd = new OracleCommand())
                    {
                        getPriceCmd.CommandText = getProductPriceQuery;
                        getPriceCmd.Connection = conn;
                        getPriceCmd.Transaction = transaction;
                        getPriceCmd.Parameters.Add(":p_productId", OracleDbType.Int32).Value = productId;
                        object priceResult = getPriceCmd.ExecuteScalar();
                        if (priceResult != DBNull.Value && priceResult != null)
                        {
                            unitPrice = Convert.ToDecimal(priceResult);
                        }
                    }

                    // 3. Check if the product already exists in OrderDetails for this OrderID
                    string selectOrderDetailQuery = "SELECT QUANTITY FROM TUNG.ORDERDETAILS WHERE ORDERID = :p_orderId AND PRODUCTID = :p_productId";
                    using (OracleCommand selectODCmd = new OracleCommand())
                    {
                        selectODCmd.CommandText = selectOrderDetailQuery;
                        selectODCmd.Connection = conn;
                        selectODCmd.Transaction = transaction;
                        selectODCmd.Parameters.Add(":p_orderId", OracleDbType.Int32).Value = orderId;
                        selectODCmd.Parameters.Add(":p_productId", OracleDbType.Int32).Value = productId;
                        object existingQuantity = selectODCmd.ExecuteScalar();

                        if (existingQuantity != null)
                        {
                            // Product exists, update quantity
                            int currentQuantity = Convert.ToInt32(existingQuantity);
                            string updateOrderDetailQuery = "UPDATE TUNG.ORDERDETAILS SET QUANTITY = :p_quantity WHERE ORDERID = :p_orderId AND PRODUCTID = :p_productId";
                            using (OracleCommand updateODCmd = new OracleCommand())
                            {
                                updateODCmd.CommandText = updateOrderDetailQuery;
                                updateODCmd.Connection = conn;
                                updateODCmd.Transaction = transaction;
                                updateODCmd.Parameters.Add(":p_quantity", OracleDbType.Int32).Value = currentQuantity + quantity;
                                updateODCmd.Parameters.Add(":p_orderId", OracleDbType.Int32).Value = orderId;
                                updateODCmd.Parameters.Add(":p_productId", OracleDbType.Int32).Value = productId;
                                updateODCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Product does not exist, insert new OrderDetail
                            string insertOrderDetailQuery = "INSERT INTO TUNG.ORDERDETAILS (ORDERID, PRODUCTID, UNITPRICE, QUANTITY, DISCOUNT, NOTE) VALUES (:p_orderId, :p_productId, :p_unitPrice, :p_quantity, :p_discount, :p_note)";
                            using (OracleCommand insertODCmd = new OracleCommand())
                            {
                                insertODCmd.CommandText = insertOrderDetailQuery;
                                insertODCmd.Connection = conn;
                                insertODCmd.Transaction = transaction;
                                insertODCmd.Parameters.Add(":p_orderId", OracleDbType.Int32).Value = orderId;
                                insertODCmd.Parameters.Add(":p_productId", OracleDbType.Int32).Value = productId;
                                insertODCmd.Parameters.Add(":p_unitPrice", OracleDbType.Decimal).Value = unitPrice;
                                insertODCmd.Parameters.Add(":p_quantity", OracleDbType.Int32).Value = quantity;
                                insertODCmd.Parameters.Add(":p_discount", OracleDbType.Double).Value = discount; // Assuming 0 for cart items
                                insertODCmd.Parameters.Add(":p_note", OracleDbType.Clob).Value = DBNull.Value; // No note for cart items
                                insertODCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    System.Diagnostics.Debug.WriteLine($"Error adding to cart: {ex.Message}");
                    throw; 
                }
            }
        }

        private int GetOrCreatePendingOrder(int employeeId, OracleConnection conn, OracleTransaction transaction)
        {
            int orderId = 0;
            // Try to find an existing pending order (SHIPPEDDATE IS NULL)
            string selectOrderQuery = "SELECT ORDERID FROM TUNG.ORDERS WHERE EMPLOYEEID = :p_employeeId AND SHIPPEDDATE IS NULL";
            using (OracleCommand cmd = new OracleCommand())
            {
                cmd.CommandText = selectOrderQuery;
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.Parameters.Add(":p_employeeId", OracleDbType.Int32).Value = employeeId;
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    orderId = Convert.ToInt32(result);
                }
            }

            if (orderId == 0)
            {
                // Create a new order if no pending order exists
                string insertOrderQuery = "INSERT INTO TUNG.ORDERS (ORDERID, EMPLOYEEID, ORDERDATE, REQUIREDDATE, SHIPNAME, SHIPADDRESS) VALUES (TUNG.SEQ_NW_ORDERS.NEXTVAL, :p_employeeId, :p_orderDate, :p_requiredDate, :p_shipName, :p_shipAddress) RETURNING ORDERID INTO :p_orderId";
                using (OracleCommand insertCmd = new OracleCommand())
                {
                    insertCmd.CommandText = insertOrderQuery;
                    insertCmd.Connection = conn;
                    insertCmd.Transaction = transaction;
                    insertCmd.Parameters.Add(":p_employeeId", OracleDbType.Int32).Value = employeeId;
                    insertCmd.Parameters.Add(":p_orderDate", OracleDbType.Date).Value = DateTime.Now;
                    insertCmd.Parameters.Add(":p_requiredDate", OracleDbType.Date).Value = DateTime.Now.AddDays(7); // Example: Required within 7 days
                    insertCmd.Parameters.Add(":p_shipName", OracleDbType.Varchar2).Value = "N/A"; // Placeholder
                    insertCmd.Parameters.Add(":p_shipAddress", OracleDbType.Varchar2).Value = "N/A"; // Placeholder

                    OracleParameter outParam = new OracleParameter(":p_orderId", OracleDbType.Int32, ParameterDirection.ReturnValue);
                    insertCmd.Parameters.Add(outParam);

                    insertCmd.ExecuteNonQuery();
                    orderId = Convert.ToInt32(outParam.Value.ToString());
                }
            }
            return orderId;
        }

        public List<CartItem> GetCartItems(int employeeId)
        {
            List<CartItem> cartItems = new List<CartItem>();
            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                try
                {
                    // Find the pending OrderID for the employee
                    int orderId = 0;
                    string selectOrderQuery = "SELECT ORDERID FROM TUNG.ORDERS WHERE EMPLOYEEID = :p_employeeId AND SHIPPEDDATE IS NULL";
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.CommandText = selectOrderQuery;
                        cmd.Connection = conn;
                        cmd.Parameters.Add(":p_employeeId", OracleDbType.Int32).Value = employeeId;
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            orderId = Convert.ToInt32(result);
                        }
                    }

                    if (orderId == 0)
                    {
                        return cartItems; // No pending order, so cart is empty
                    }

                    // Retrieve OrderDetails for the pending OrderID
                    string query = @"
                        SELECT od.PRODUCTID, od.QUANTITY, od.UNITPRICE,
                               p.PRODUCTNAME, p.UNITSINSTOCK,
                               (SELECT pi.IMAGEURL FROM TUNG.PRODUCT_IMAGES pi WHERE pi.PRODUCTID = p.PRODUCTID AND ROWNUM = 1) AS ImageUrl
                        FROM TUNG.ORDERDETAILS od
                        JOIN TUNG.PRODUCTS p ON od.PRODUCTID = p.PRODUCTID
                        WHERE od.ORDERID = :p_orderId";

                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = conn;
                        cmd.Parameters.Add(":p_orderId", OracleDbType.Int32).Value = orderId;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cartItems.Add(new CartItem
                                {
                                    ProductID = Convert.ToInt32(reader["PRODUCTID"]),
                                    Quantity = Convert.ToInt32(reader["QUANTITY"]),
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    UnitPrice = Convert.ToDecimal(reader["UNITPRICE"]),
                                    ImageUrl = reader["ImageUrl"] != DBNull.Value ? reader["ImageUrl"].ToString() : "",
                                    UnitsInStock = Convert.ToInt32(reader["UNITSINSTOCK"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error getting cart items: {ex.Message}");
                    throw;
                }
            }
            return cartItems;
        }

        public void UpdateCartItemQuantity(int employeeId, int productId, int newQuantity)
        {
            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                OracleTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    int orderId = GetPendingOrderId(employeeId, conn, transaction);

                    if (orderId == 0) // No pending order found, nothing to update/remove
                    {
                        transaction.Rollback();
                        return;
                    }

                    if (newQuantity <= 0)
                    {
                        // If quantity is 0 or less, remove the item
                        RemoveCartItem(employeeId, productId, conn, transaction); // Pass transaction here
                    }
                    else
                    {
                        string updateQuery = "UPDATE TUNG.ORDERDETAILS SET QUANTITY = :p_quantity WHERE ORDERID = :p_orderId AND PRODUCTID = :p_productId";
                        using (OracleCommand cmd = new OracleCommand())
                        {
                            cmd.CommandText = updateQuery;
                            cmd.Connection = conn;
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(":p_quantity", OracleDbType.Int32).Value = newQuantity;
                            cmd.Parameters.Add(":p_orderId", OracleDbType.Int32).Value = orderId;
                            cmd.Parameters.Add(":p_productId", OracleDbType.Int32).Value = productId;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    System.Diagnostics.Debug.WriteLine($"Error updating cart item quantity: {ex.Message}");
                    throw;
                }
            }
        }

        public void RemoveCartItem(int employeeId, int productId)
        {
            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                OracleTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    RemoveCartItem(employeeId, productId, conn, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    System.Diagnostics.Debug.WriteLine($"Error removing cart item: {ex.Message}");
                    throw;
                }
            }
        }

        private void RemoveCartItem(int employeeId, int productId, OracleConnection conn, OracleTransaction transaction)
        {
            int orderId = GetPendingOrderId(employeeId, conn, transaction);

            if (orderId == 0) return; // No pending order found, nothing to remove

            string deleteQuery = "DELETE FROM TUNG.ORDERDETAILS WHERE ORDERID = :p_orderId AND PRODUCTID = :p_productId";
            using (OracleCommand cmd = new OracleCommand())
            {
                cmd.CommandText = deleteQuery;
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.Parameters.Add(":p_orderId", OracleDbType.Int32).Value = orderId;
                cmd.Parameters.Add(":p_productId", OracleDbType.Int32).Value = productId;
                cmd.ExecuteNonQuery();
            }
        }

        // Helper method to get pending order ID (similar to GetOrCreatePendingOrder but doesn't create)
        private int GetPendingOrderId(int employeeId, OracleConnection conn, OracleTransaction transaction)
        {
            string selectOrderQuery = "SELECT ORDERID FROM TUNG.ORDERS WHERE EMPLOYEEID = :p_employeeId AND SHIPPEDDATE IS NULL";
            using (OracleCommand cmd = new OracleCommand())
            {
                cmd.CommandText = selectOrderQuery;
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.Parameters.Add(":p_employeeId", OracleDbType.Int32).Value = employeeId;
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }
            return 0;
        }
    }

    public class CartItem
    {
        // We don't need ShoppingCartID or UserID anymore as we are using OrderDetails
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; }
        public int UnitsInStock { get; set; }
    }
}