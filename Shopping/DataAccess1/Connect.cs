using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Shopping.DataAccess1
{
    public class Connect
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;
        private static Connect instance;
        private static readonly object lockObject = new object();

        private Connect() { }

        public static Connect Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Connect();
                        }
                    }
                }
                return instance;
            }
        }

        public OracleConnection GetConnection()
        {
            try
            {
                var connection = new OracleConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening database connection: " + ex.Message);
            }
        }

        // Optional helper methods for common database operations
        public OracleDataReader ExecuteQuery(string query, OracleConnection connection)
        {
            try
            {
                OracleCommand command = new OracleCommand(query, connection);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query: " + ex.Message);
            }
        }

        public int ExecuteNonQuery(string query, OracleConnection connection)
        {
            try
            {
                OracleCommand command = new OracleCommand(query, connection);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing non-query: " + ex.Message);
            }
        }

        public object ExecuteScalar(string query, OracleConnection connection)
        {
            try
            {
                OracleCommand command = new OracleCommand(query, connection);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing scalar query: " + ex.Message);
            }
        }
    }
}