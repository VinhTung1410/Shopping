using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Shopping.DataAccess1
{
    public class Connect
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;
        private static Connect instance;
        private OracleConnection connection;

        private Connect()
        {
            try
            {
                connection = new OracleConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing database connection: " + ex.Message);
            }
        }

        public static Connect Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Connect();
                }
                return instance;
            }
        }

        public OracleConnection GetConnection()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening database connection: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error closing database connection: " + ex.Message);
            }
        }

        // Method to execute a query and return results
        public OracleDataReader ExecuteQuery(string query)
        {
            try
            {
                OracleCommand command = new OracleCommand(query, GetConnection());
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query: " + ex.Message);
            }
        }

        // Method to execute a non-query command (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query)
        {
            try
            {
                OracleCommand command = new OracleCommand(query, GetConnection());
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing non-query: " + ex.Message);
            }
        }

        // Method to execute a scalar query
        public object ExecuteScalar(string query)
        {
            try
            {
                OracleCommand command = new OracleCommand(query, GetConnection());
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing scalar query: " + ex.Message);
            }
        }
    }
}