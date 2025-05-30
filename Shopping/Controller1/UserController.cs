using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Shopping.DataAccess1;
using Shopping.Model1;
using System.Diagnostics;

namespace Shopping.Controller1
{
    public class UserController
    {
        public bool IsUsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM USERS WHERE USERNAME = :Username";
            try
            {
                using (OracleConnection conn = Connect.Instance.GetConnection())
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("Username", OracleDbType.Varchar2).Value = username;
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking username: " + ex.Message);
            }
            finally
            {
                Connect.Instance.CloseConnection();
            }

        }

        public bool IsEmailExists(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string query = "SELECT COUNT(*) FROM USERS WHERE UPPER(EMAIL) = UPPER(:Email)";
            try
            {
                using (OracleConnection conn = Connect.Instance.GetConnection())
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("Email", OracleDbType.Varchar2).Value = email.Trim();
                        object result = cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine($"Email check for {email}: Result = {result}");

                        if (result != null)
                        {
                            int count = Convert.ToInt32(result);
                            return count > 0;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking email: {ex.Message}");
                // Trong trường hợp lỗi, cho phép tiếp tục đăng ký
                return false;
            }
            finally
            {
                Connect.Instance.CloseConnection();
            }

        }

        public bool RegisterUser(User user, Employee employee)
        {
            using (OracleConnection conn = Connect.Instance.GetConnection())
            {
                OracleTransaction transaction = conn.BeginTransaction();
                try
                {


                    // 1.Insert Employee
                    string insertEmployeeQuery = @"INSERT INTO EMPLOYEES
                        (EMPLOYEEID, FIRSTNAME, LASTNAME, ADDRESS) 
                        VALUES (SEQ_NW_EMPLOYEES.nextval, :FirstName, :LastName, :Address)";

                    using (OracleCommand cmd = new OracleCommand(insertEmployeeQuery, conn))
                    {
                        cmd.Transaction = transaction;
                        //cmd.Parameters.Add("EmployeeID", OracleDbType.Int32).Value = employee.EmployeeID;
                        cmd.Parameters.Add("FirstName", OracleDbType.Varchar2).Value = employee.FirstName;
                        cmd.Parameters.Add("LastName", OracleDbType.Varchar2).Value = employee.LastName;
                        cmd.Parameters.Add("Address", OracleDbType.Varchar2).Value = employee.Address;
                        cmd.ExecuteNonQuery();
                    }
                    //2. Get current sequence in #1

                    // Get next EmployeeID
                    string getNextEmpIdQuery = "select SEQ_NW_EMPLOYEES.currval from dual ";
                    using (OracleCommand cmd = new OracleCommand(getNextEmpIdQuery, conn))
                    {
                        employee.EmployeeID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // 3. Insert User
                    string insertUserQuery = @"INSERT INTO USERS
                        (UserId, USERNAME, PASSWORDHASH, EMAIL, ROLEID, EMPLOYEEID, CREATEDAT, ISACTIVE) 
                        VALUES (SEQ_NW_USERS.nextval, :Username, :PASSWORDHASH, :Email, :RoleID, :EmployeeID, :CreatedAt, :IsActive)";

                    using (OracleCommand cmd = new OracleCommand(insertUserQuery, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add("Username", OracleDbType.Varchar2).Value = user.UserName;
                        cmd.Parameters.Add("PASSWORDHASH", OracleDbType.Varchar2).Value = user.Password;
                        cmd.Parameters.Add("Email", OracleDbType.Varchar2).Value = user.Email;
                        cmd.Parameters.Add("RoleID", OracleDbType.Int32).Value = user.RoleID;
                        cmd.Parameters.Add("EmployeeID", OracleDbType.Int32).Value = employee.EmployeeID;
                        cmd.Parameters.Add("CreatedAt", OracleDbType.Date).Value = user.CreatedAt;
                        cmd.Parameters.Add("IsActive", OracleDbType.Int32).Value = user.IsActive ? 1 : 0;
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error registering user: " + ex.Message);
                }
                finally
                {
                    Connect.Instance.CloseConnection();
                }
            }
        }

        public bool ValidateLogin(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM USERS WHERE USERNAME = :Username AND PASSWORDHASH = :PASSWORDHASH";
            try
            {
                using (OracleConnection conn = Connect.Instance.GetConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("Username", OracleDbType.Varchar2).Value = username;
                        cmd.Parameters.Add("PASSWORDHASH", OracleDbType.Varchar2).Value = password;
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating login: " + ex.Message);
            }
        }
    }
}