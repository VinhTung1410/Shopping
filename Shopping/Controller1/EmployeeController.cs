using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Shopping.Model1;

namespace Shopping.Controller1
{
    public class EmployeeController
    {
        private readonly string connectionString;

        public EmployeeController()
        {
            var connStr = ConfigurationManager.ConnectionStrings["OracleConn"];
            if (connStr == null)
            {
                throw new ConfigurationErrorsException("Connection string 'OracleConn' not found in Web.config");
            }
            connectionString = connStr.ConnectionString;
        }

        // Get all employees with their user and role information
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            e.EMPLOYEEID,
                            e.FIRSTNAME,
                            e.LASTNAME,
                            e.TITLE,
                            e.TITLEOFCOURTESY,
                            e.BIRTHDATE,
                            e.HIREDATE,
                            e.ADDRESS,
                            e.CITY,
                            e.REGION,
                            e.POSTALCODE,
                            e.COUNTRY,
                            e.HOMEPHONE,
                            e.EXTENSION,
                            e.REPORTSTO,
                            u.USERNAME,
                            u.EMAIL,
                            u.ISACTIVE,
                            u.CREATEDAT,
                            u.UPDATEDAT,
                            u.ROLEID,
                            r.ROLENAME
                        FROM TUNG.EMPLOYEES e
                        LEFT JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                        LEFT JOIN TUNG.ROLES r ON u.ROLEID = r.ROLEID";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapEmployeeFromReader(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAllEmployees: {ex.Message}");
                throw;
            }
            return employees;
        }

        // Get employee by ID
        public Employee GetEmployeeById(int employeeId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            e.EMPLOYEEID,
                            e.FIRSTNAME,
                            e.LASTNAME,
                            e.TITLE,
                            e.TITLEOFCOURTESY,
                            e.BIRTHDATE,
                            e.HIREDATE,
                            e.ADDRESS,
                            e.CITY,
                            e.REGION,
                            e.POSTALCODE,
                            e.COUNTRY,
                            e.HOMEPHONE,
                            e.EXTENSION,
                            e.REPORTSTO,
                            u.USERNAME,
                            u.EMAIL,
                            u.ISACTIVE,
                            u.CREATEDAT,
                            u.UPDATEDAT,
                            u.ROLEID,
                            r.ROLENAME
                        FROM TUNG.EMPLOYEES e
                        LEFT JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                        LEFT JOIN TUNG.ROLES r ON u.ROLEID = r.ROLEID
                        WHERE e.EMPLOYEEID = :EmployeeID";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employeeId;
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapEmployeeFromReader(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetEmployeeById: {ex.Message}");
                throw;
            }
            return null;
        }

        // Create new employee with user account
        public int CreateEmployee(Employee employee)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insert into EMPLOYEES
                            string empQuery = @"
                                INSERT INTO TUNG.EMPLOYEES 
                                (EMPLOYEEID, FIRSTNAME, LASTNAME, TITLE, TITLEOFCOURTESY, 
                                BIRTHDATE, HIREDATE, ADDRESS, CITY, REGION, POSTALCODE, 
                                COUNTRY, HOMEPHONE, EXTENSION, REPORTSTO)
                                VALUES 
                                (SEQ_NW_EMPLOYEES.NEXTVAL, :FirstName, :LastName, :Title, :TitleOfCourtesy,
                                :BirthDate, :HireDate, :Address, :City, :Region, :PostalCode,
                                :Country, :HomePhone, :Extension, :ReportsTo)
                                RETURNING EMPLOYEEID INTO :EmployeeID";

                            int newEmployeeId;
                            using (OracleCommand empCmd = new OracleCommand(empQuery, conn))
                            {
                                empCmd.Transaction = transaction;

                                empCmd.Parameters.Add(":FirstName", OracleDbType.Varchar2).Value = employee.FirstName;
                                empCmd.Parameters.Add(":LastName", OracleDbType.Varchar2).Value = employee.LastName;
                                empCmd.Parameters.Add(":Title", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Title) ? (object)DBNull.Value : employee.Title;
                                empCmd.Parameters.Add(":TitleOfCourtesy", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.TitleOfCourtesy) ? (object)DBNull.Value : employee.TitleOfCourtesy;
                                empCmd.Parameters.Add(":BirthDate", OracleDbType.Date).Value = !employee.BirthDate.HasValue ? (object)DBNull.Value : employee.BirthDate.Value;
                                empCmd.Parameters.Add(":HireDate", OracleDbType.Date).Value = !employee.HireDate.HasValue ? (object)DBNull.Value : employee.HireDate.Value;
                                empCmd.Parameters.Add(":Address", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Address) ? (object)DBNull.Value : employee.Address;
                                empCmd.Parameters.Add(":City", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.City) ? (object)DBNull.Value : employee.City;
                                empCmd.Parameters.Add(":Region", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Region) ? (object)DBNull.Value : employee.Region;
                                empCmd.Parameters.Add(":PostalCode", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.PostalCode) ? (object)DBNull.Value : employee.PostalCode;
                                empCmd.Parameters.Add(":Country", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Country) ? (object)DBNull.Value : employee.Country;
                                empCmd.Parameters.Add(":HomePhone", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.HomePhone) ? (object)DBNull.Value : employee.HomePhone;
                                empCmd.Parameters.Add(":Extension", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Extension) ? (object)DBNull.Value : employee.Extension;
                                empCmd.Parameters.Add(":ReportsTo", OracleDbType.Int32).Value = !employee.ReportsTo.HasValue ? (object)DBNull.Value : employee.ReportsTo.Value;

                                // Output parameter for the new EMPLOYEEID
                                OracleParameter paramEmployeeId = new OracleParameter(":EmployeeID", OracleDbType.Int32);
                                paramEmployeeId.Direction = ParameterDirection.Output;
                                empCmd.Parameters.Add(paramEmployeeId);

                                empCmd.ExecuteNonQuery();
                                newEmployeeId = Convert.ToInt32(paramEmployeeId.Value.ToString());
                            }

                            // Insert into USERS
                            string userQuery = @"
                                INSERT INTO TUNG.USERS 
                                (USERID, ROLEID, EMPLOYEEID, USERNAME, PASSWORDHASH, EMAIL, ISACTIVE, CREATEDAT)
                                VALUES 
                                (SEQ_NW_USERS.NEXTVAL, :RoleID, :EmployeeID, :Username, :PasswordHash, :Email, :IsActive, SYSDATE)";

                            using (OracleCommand userCmd = new OracleCommand(userQuery, conn))
                            {
                                userCmd.Transaction = transaction;

                                // Ensure proper number handling for all numeric parameters
                                userCmd.Parameters.Add(":RoleID", OracleDbType.Int32).Value = employee.RoleID;
                                userCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = newEmployeeId;
                                userCmd.Parameters.Add(":Username", OracleDbType.Varchar2).Value = employee.Username;
                                userCmd.Parameters.Add(":PasswordHash", OracleDbType.Varchar2).Value = employee.PasswordHash;
                                userCmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Email) ? (object)DBNull.Value : employee.Email;
                                userCmd.Parameters.Add(":IsActive", OracleDbType.Int32).Value = employee.IsActive ? 1 : 0;

                                userCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return newEmployeeId;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CreateEmployee: {ex.Message}");
                throw;
            }
        }

        // Update employee and user information
        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update EMPLOYEES
                            string empQuery = @"
                                UPDATE TUNG.EMPLOYEES SET 
                                FIRSTNAME = :FirstName,
                                LASTNAME = :LastName,
                                TITLE = :Title,
                                TITLEOFCOURTESY = :TitleOfCourtesy,
                                BIRTHDATE = :BirthDate,
                                HIREDATE = :HireDate,
                                ADDRESS = :Address,
                                CITY = :City,
                                REGION = :Region,
                                POSTALCODE = :PostalCode,
                                COUNTRY = :Country,
                                HOMEPHONE = :HomePhone,
                                EXTENSION = :Extension,
                                REPORTSTO = :ReportsTo
                                WHERE EMPLOYEEID = :EmployeeID";

                            using (OracleCommand empCmd = new OracleCommand(empQuery, conn))
                            {
                                empCmd.Transaction = transaction;

                                empCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employee.EmployeeID;
                                empCmd.Parameters.Add(":FirstName", OracleDbType.Varchar2).Value = employee.FirstName;
                                empCmd.Parameters.Add(":LastName", OracleDbType.Varchar2).Value = employee.LastName;
                                empCmd.Parameters.Add(":Title", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Title) ? (object)DBNull.Value : employee.Title;
                                empCmd.Parameters.Add(":TitleOfCourtesy", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.TitleOfCourtesy) ? (object)DBNull.Value : employee.TitleOfCourtesy;
                                empCmd.Parameters.Add(":BirthDate", OracleDbType.Date).Value = !employee.BirthDate.HasValue ? (object)DBNull.Value : employee.BirthDate.Value;
                                empCmd.Parameters.Add(":HireDate", OracleDbType.Date).Value = !employee.HireDate.HasValue ? (object)DBNull.Value : employee.HireDate.Value;
                                empCmd.Parameters.Add(":Address", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Address) ? (object)DBNull.Value : employee.Address;
                                empCmd.Parameters.Add(":City", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.City) ? (object)DBNull.Value : employee.City;
                                empCmd.Parameters.Add(":Region", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Region) ? (object)DBNull.Value : employee.Region;
                                empCmd.Parameters.Add(":PostalCode", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.PostalCode) ? (object)DBNull.Value : employee.PostalCode;
                                empCmd.Parameters.Add(":Country", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Country) ? (object)DBNull.Value : employee.Country;
                                empCmd.Parameters.Add(":HomePhone", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.HomePhone) ? (object)DBNull.Value : employee.HomePhone;
                                empCmd.Parameters.Add(":Extension", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Extension) ? (object)DBNull.Value : employee.Extension;
                                empCmd.Parameters.Add(":ReportsTo", OracleDbType.Int32).Value = !employee.ReportsTo.HasValue ? (object)DBNull.Value : employee.ReportsTo.Value;

                                empCmd.ExecuteNonQuery();
                            }

                            // Update USERS
                            string userQuery = @"
                                UPDATE TUNG.USERS SET 
                                ROLEID = :RoleID,
                                EMAIL = :Email,
                                ISACTIVE = :IsActive,
                                UPDATEDAT = SYSDATE
                                WHERE EMPLOYEEID = :EmployeeID";

                            using (OracleCommand userCmd = new OracleCommand(userQuery, conn))
                            {
                                userCmd.Transaction = transaction;

                                userCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employee.EmployeeID;
                                userCmd.Parameters.Add(":RoleID", OracleDbType.Int32).Value = employee.RoleID;
                                userCmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(employee.Email) ? (object)DBNull.Value : employee.Email;
                                userCmd.Parameters.Add(":IsActive", OracleDbType.Int32).Value = employee.IsActive ? 1 : 0;

                                userCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateEmployee: {ex.Message}");
                throw;
            }
        }

        // Toggle employee active status
        public bool ToggleEmployeeStatus(int employeeId, bool isActive)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                        UPDATE TUNG.USERS SET 
                        ISACTIVE = :IsActive,
                        UPDATEDAT = SYSDATE
                        WHERE EMPLOYEEID = :EmployeeID";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employeeId;
                        cmd.Parameters.Add(":IsActive", OracleDbType.Int32).Value = isActive ? 1 : 0;

                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ToggleEmployeeStatus: {ex.Message}");
                throw;
            }
        }

        // Helper method to map reader to Employee object
        private Employee MapEmployeeFromReader(OracleDataReader reader)
        {
            try
            {
                var employee = new Employee
                {
                    EmployeeID = Convert.ToInt32(reader["EMPLOYEEID"]),
                    FirstName = reader["FIRSTNAME"].ToString(),
                    LastName = reader["LASTNAME"].ToString(),
                    Title = reader["TITLE"] != DBNull.Value ? reader["TITLE"].ToString() : null,
                    TitleOfCourtesy = reader["TITLEOFCOURTESY"] != DBNull.Value ? reader["TITLEOFCOURTESY"].ToString() : null,
                    BirthDate = reader["BIRTHDATE"] != DBNull.Value ? Convert.ToDateTime(reader["BIRTHDATE"]) : (DateTime?)null,
                    HireDate = reader["HIREDATE"] != DBNull.Value ? Convert.ToDateTime(reader["HIREDATE"]) : (DateTime?)null,
                    Address = reader["ADDRESS"] != DBNull.Value ? reader["ADDRESS"].ToString() : null,
                    City = reader["CITY"] != DBNull.Value ? reader["CITY"].ToString() : null,
                    Region = reader["REGION"] != DBNull.Value ? reader["REGION"].ToString() : null,
                    PostalCode = reader["POSTALCODE"] != DBNull.Value ? reader["POSTALCODE"].ToString() : null,
                    Country = reader["COUNTRY"] != DBNull.Value ? reader["COUNTRY"].ToString() : null,
                    HomePhone = reader["HOMEPHONE"] != DBNull.Value ? reader["HOMEPHONE"].ToString() : null,
                    Extension = reader["EXTENSION"] != DBNull.Value ? reader["EXTENSION"].ToString() : null,
                    ReportsTo = reader["REPORTSTO"] != DBNull.Value ? Convert.ToInt32(reader["REPORTSTO"]) : (int?)null,
                    Username = reader["USERNAME"] != DBNull.Value ? reader["USERNAME"].ToString() : null,
                    Email = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : null,
                    RoleName = reader["ROLENAME"] != DBNull.Value ? reader["ROLENAME"].ToString() : null,
                    IsActive = reader["ISACTIVE"] != DBNull.Value && Convert.ToInt32(reader["ISACTIVE"]) == 1,
                    CreatedAt = reader["CREATEDAT"] != DBNull.Value ? Convert.ToDateTime(reader["CREATEDAT"]) : DateTime.MinValue,
                    UpdatedAt = reader["UPDATEDAT"] != DBNull.Value ? Convert.ToDateTime(reader["UPDATEDAT"]) : (DateTime?)null,
                    RoleID = reader["ROLEID"] != DBNull.Value ? Convert.ToInt32(reader["ROLEID"]) : 2 // Default to User role if null
                };
                return employee;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in MapEmployeeFromReader: {ex.Message}");
                throw;
            }
        }
    }
}