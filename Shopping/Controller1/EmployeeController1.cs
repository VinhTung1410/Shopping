using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Diagnostics;
using Shopping.Model1;

namespace Shopping.Controller1
{
    public class EmployeeController1
    {
        private readonly string _connectionString;

        public EmployeeController1()
        {
            string connName = "OracleConn";
            _connectionString = ConfigurationManager.ConnectionStrings[connName]?.ConnectionString;

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ConfigurationErrorsException($"Connection string '{connName}' not found or is empty in the configuration file.");
            }
        }

        private Employee MapEmployeeFromReader(OracleDataReader reader)
        {
            Employee employee = new Employee
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EMPLOYEEID")),
                LastName = reader["LASTNAME"] as string,
                FirstName = reader["FIRSTNAME"] as string,
                Title = reader["TITLE"] as string,
                TitleOfCourtesy = reader["TITLEOFCOURTESY"] as string,
                Address = reader["ADDRESS"] as string,
                City = reader["CITY"] as string,
                Region = reader["REGION"] as string,
                PostalCode = reader["POSTALCODE"] as string,
                Country = reader["COUNTRY"] as string,
                HomePhone = reader["HOMEPHONE"] as string,
                Extension = reader["EXTENSION"] as string,
                ReportsTo = reader["REPORTSTO"] as object == DBNull.Value ? (int?)null : Convert.ToInt32(reader["REPORTSTO"])
            };

            if (reader["BIRTHDATE"] as object != DBNull.Value)
            {
                employee.BirthDate = Convert.ToDateTime(reader["BIRTHDATE"]);
            }
            if (reader["HIREDATE"] as object != DBNull.Value)
            {
                employee.HireDate = Convert.ToDateTime(reader["HIREDATE"]);
            }

            // Map User related information directly to Employee
            if (reader["USERID"] as object != DBNull.Value)
            {
                employee.UserID = Convert.ToInt32(reader["USERID"]);
                employee.RoleID = Convert.ToInt32(reader["ROLEID"]);
                employee.Username = reader["USERNAME"] as string;
                employee.PasswordHash = reader["PASSWORDHASH"] as string;
                employee.Email = reader["EMAIL_U"] as string; // Alias for EMAIL from USERS table
                employee.IsActive = Convert.ToBoolean(reader["ISACTIVE"]);
                employee.CreatedAt = Convert.ToDateTime(reader["CREATEDAT_U"]); // Alias for CREATEDAT from USERS table
                employee.UpdatedAt = reader["UPDATEDAT_U"] as object == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UPDATEDAT_U"]);
            }
            
            // Map RoleName if available
            if (reader["ROLENAME"] as object != DBNull.Value)
            {
                employee.RoleName = reader["ROLENAME"] as string;
            }

            return employee;
        }
        #region Show all Employees and click on EmployeeID to view details
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT 
                                e.EMPLOYEEID, e.LASTNAME, e.FIRSTNAME, e.TITLE, e.TITLEOFCOURTESY, e.BIRTHDATE, e.HIREDATE, e.ADDRESS, e.CITY, e.REGION, e.POSTALCODE, e.COUNTRY, e.HOMEPHONE, e.EXTENSION, e.REPORTSTO, 
                                u.USERID, u.ROLEID, u.USERNAME, u.PASSWORDHASH, u.EMAIL AS EMAIL_U, u.ISACTIVE, u.CREATEDAT AS CREATEDAT_U, u.UPDATEDAT AS UPDATEDAT_U,
                                r.ROLENAME
                                FROM TUNG.EMPLOYEES e
                                LEFT JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                                LEFT JOIN TUNG.ROLES r ON u.ROLEID = r.ROLEID";
                    using (OracleCommand command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = query;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapEmployeeFromReader(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in GetAllEmployees: {ex.Message}");
                    throw; 
                }
            }
            return employees;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT 
                                e.EMPLOYEEID, e.LASTNAME, e.FIRSTNAME, e.TITLE, e.TITLEOFCOURTESY, e.BIRTHDATE, e.HIREDATE, e.ADDRESS, e.CITY, e.REGION, e.POSTALCODE, e.COUNTRY, e.HOMEPHONE, e.EXTENSION, e.REPORTSTO, 
                                u.USERID, u.ROLEID, u.USERNAME, u.PASSWORDHASH, u.EMAIL AS EMAIL_U, u.ISACTIVE, u.CREATEDAT AS CREATEDAT_U, u.UPDATEDAT AS UPDATEDAT_U,
                                r.ROLENAME
                                FROM TUNG.EMPLOYEES e
                                LEFT JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                                LEFT JOIN TUNG.ROLES r ON u.ROLEID = r.ROLEID
                                WHERE e.EMPLOYEEID = :EmployeeID";
                    using (OracleCommand command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = query;
                        command.Parameters.Add(new OracleParameter("EmployeeID", employeeId));
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employee = MapEmployeeFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in GetEmployeeById: {ex.Message}");
                    throw;
                }
            }
            return employee;
        }
        #endregion

        #region Create Employee
        public int CreateEmployee(Employee employee)
        {
            int newEmployeeId = 0;
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                OracleTransaction transaction = connection.BeginTransaction();
                try
                {
                    // Insert into EMPLOYEES
                    string insertEmployeeQuery = @"INSERT INTO TUNG.EMPLOYEES 
                                                (EMPLOYEEID, LASTNAME, FIRSTNAME, TITLE, TITLEOFCOURTESY, BIRTHDATE, HIREDATE, ADDRESS, CITY, REGION, POSTALCODE, COUNTRY, HOMEPHONE, EXTENSION, REPORTSTO)
                                                VALUES (SEQ_NW_EMPLOYEES.NEXTVAL, :LastName, :FirstName, :Title, :TitleOfCourtesy, :BirthDate, :HireDate, :Address, :City, :Region, :PostalCode, :Country, :HomePhone, :Extension, :ReportsTo)
                                                RETURNING EMPLOYEEID INTO :EmployeeID";
                    using (OracleCommand command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = insertEmployeeQuery;
                        command.Parameters.Add(new OracleParameter("LastName", employee.LastName));
                        command.Parameters.Add(new OracleParameter("FirstName", employee.FirstName));
                        command.Parameters.Add(new OracleParameter("Title", employee.Title));
                        command.Parameters.Add(new OracleParameter("TitleOfCourtesy", employee.TitleOfCourtesy));
                        command.Parameters.Add(new OracleParameter("BirthDate", employee.BirthDate));
                        command.Parameters.Add(new OracleParameter("HireDate", employee.HireDate));
                        command.Parameters.Add(new OracleParameter("Address", employee.Address));
                        command.Parameters.Add(new OracleParameter("City", employee.City));
                        command.Parameters.Add(new OracleParameter("Region", employee.Region));
                        command.Parameters.Add(new OracleParameter("PostalCode", employee.PostalCode));
                        command.Parameters.Add(new OracleParameter("Country", employee.Country));
                        command.Parameters.Add(new OracleParameter("HomePhone", employee.HomePhone));
                        command.Parameters.Add(new OracleParameter("Extension", employee.Extension));
                        command.Parameters.Add(new OracleParameter("ReportsTo", employee.ReportsTo));

                        OracleParameter employeeIdParam = new OracleParameter("EmployeeID", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                        command.Parameters.Add(employeeIdParam);

                        command.ExecuteNonQuery();
                        newEmployeeId = ((OracleDecimal)employeeIdParam.Value).ToInt32();
                    }

                    // Insert into USERS (assuming Employee contains all necessary User fields)
                    string insertUserQuery = @"INSERT INTO TUNG.USERS 
                                                (USERID, ROLEID, EMPLOYEEID, USERNAME, PASSWORDHASH, EMAIL, ISACTIVE, CREATEDAT, UPDATEDAT)
                                                VALUES (SEQ_NW_USERS.NEXTVAL, :RoleId, :EmployeeId, :Username, :PasswordHash, :Email, :IsActive, SYSDATE, SYSDATE)";
                    using (OracleCommand command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = insertUserQuery;
                        command.Parameters.Add(new OracleParameter("RoleId", employee.RoleID));
                        command.Parameters.Add(new OracleParameter("EmployeeId", newEmployeeId));
                        command.Parameters.Add(new OracleParameter("Username", employee.Username));
                        command.Parameters.Add(new OracleParameter("PasswordHash", employee.PasswordHash));
                        command.Parameters.Add(new OracleParameter("Email", employee.Email));
                        command.Parameters.Add(new OracleParameter("IsActive", employee.IsActive ? 1 : 0)); 
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    Debug.WriteLine($"Successfully created employee with ID: {newEmployeeId}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine($"Error in CreateEmployee: {ex.Message}");
                    throw;
                }
            }
            return newEmployeeId;
        }
        #endregion

        #region Update Employee
        public bool UpdateEmployee(Employee employee)
        {
            bool success = false;
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                OracleTransaction transaction = connection.BeginTransaction();
                try
                {
                    // 1. Verify and Lock current data for both EMPLOYEE and USER
                    string selectForUpdateQuery = @"SELECT 
                                                    e.EMPLOYEEID, e.LASTNAME, e.FIRSTNAME, e.TITLE, e.TITLEOFCOURTESY, e.BIRTHDATE, e.HIREDATE, e.ADDRESS, e.CITY, e.REGION, e.POSTALCODE, e.COUNTRY, e.HOMEPHONE, e.EXTENSION, e.REPORTSTO, 
                                                    u.USERID, u.ROLEID, u.USERNAME, u.PASSWORDHASH, u.EMAIL AS EMAIL_U, u.ISACTIVE, u.CREATEDAT AS CREATEDAT_U, u.UPDATEDAT AS UPDATEDAT_U,
                                                    r.ROLENAME
                                                    FROM TUNG.EMPLOYEES e
                                                    LEFT JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                                                    LEFT JOIN TUNG.ROLES r ON u.ROLEID = r.ROLEID
                                                    WHERE e.EMPLOYEEID = :EmployeeID FOR UPDATE";

                    Employee currentEmployee = null;
                    using (OracleCommand command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = selectForUpdateQuery;
                        command.Parameters.Add(new OracleParameter("EmployeeID", employee.EmployeeID));
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentEmployee = MapEmployeeFromReader(reader);
                            } else {
                                throw new InvalidOperationException($"Employee with ID {employee.EmployeeID} not found.");
                            }
                        }
                    }

                    // 2. Check and Update TUNG.USERS
                    bool needsUserUpdate = false;
                    // Compare relevant user fields from the Employee model
                    if (currentEmployee.RoleID != employee.RoleID ||
                        currentEmployee.IsActive != employee.IsActive ||
                        !string.Equals(currentEmployee.Email, employee.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        needsUserUpdate = true;
                    }

                    if (needsUserUpdate)
                    {
                        // Special check for deactivating last active administrator
                        if (!employee.IsActive && currentEmployee.RoleID == 1) // RoleID 1 is admin
                        {
                            string countAdminQuery = @"SELECT COUNT(*) FROM TUNG.USERS WHERE ROLEID = 1 AND ISACTIVE = 1";
                            using (OracleCommand countCmd = new OracleCommand())
                            {
                                countCmd.Connection = connection;
                                countCmd.Transaction = transaction;
                                countCmd.CommandText = countAdminQuery;
                                int activeAdmins = Convert.ToInt32(countCmd.ExecuteScalar());
                                if (activeAdmins <= 1) 
                                {
                                    throw new InvalidOperationException("Cannot deactivate the last active administrator.");
                                }
                            }
                        }

                        // Check if new RoleID exists
                        string checkRoleQuery = @"SELECT COUNT(*) FROM TUNG.ROLES WHERE ROLEID = :RoleID";
                        using (OracleCommand checkRoleCmd = new OracleCommand())
                        {
                            checkRoleCmd.Connection = connection;
                            checkRoleCmd.Transaction = transaction;
                            checkRoleCmd.CommandText = checkRoleQuery;
                            checkRoleCmd.Parameters.Add(new OracleParameter("RoleID", employee.RoleID));
                            int roleCount = Convert.ToInt32(checkRoleCmd.ExecuteScalar());
                            if (roleCount == 0)
                            {
                                throw new InvalidOperationException($"Role with ID {employee.RoleID} does not exist.");
                            }
                        }

                        string updateUserQuery = @"UPDATE TUNG.USERS 
                                                    SET EMAIL = :Email, ROLEID = :RoleId, ISACTIVE = :IsActive, UPDATEDAT = SYSDATE 
                                                    WHERE EMPLOYEEID = :EmployeeId";
                        using (OracleCommand updateCmd = new OracleCommand())
                        {
                            updateCmd.Connection = connection;
                            updateCmd.Transaction = transaction;
                            updateCmd.CommandText = updateUserQuery;
                            updateCmd.Parameters.Add(new OracleParameter("Email", employee.Email));
                            updateCmd.Parameters.Add(new OracleParameter("RoleId", employee.RoleID));
                            updateCmd.Parameters.Add(new OracleParameter("IsActive", employee.IsActive ? 1 : 0));
                            updateCmd.Parameters.Add(new OracleParameter("EmployeeId", employee.EmployeeID));
                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new InvalidOperationException("Failed to update user account or user not found during update.");
                            }
                        }

                        // Re-verify after update
                        string verifyUserQuery = @"SELECT ISACTIVE, ROLEID, EMAIL FROM TUNG.USERS WHERE EMPLOYEEID = :EmployeeId";
                        using (OracleCommand verifyCmd = new OracleCommand())
                        {
                            verifyCmd.Connection = connection;
                            verifyCmd.Transaction = transaction;
                            verifyCmd.CommandText = verifyUserQuery;
                            verifyCmd.Parameters.Add(new OracleParameter("EmployeeId", employee.EmployeeID));
                            using (OracleDataReader verifyReader = verifyCmd.ExecuteReader())
                            {
                                if (verifyReader.Read())
                                {
                                    bool verifiedIsActive = Convert.ToBoolean(verifyReader["ISACTIVE"]);
                                    int verifiedRoleId = Convert.ToInt32(verifyReader["ROLEID"]);
                                    string verifiedEmail = verifyReader["EMAIL"] as string;

                                    if (verifiedIsActive != employee.IsActive ||
                                        verifiedRoleId != employee.RoleID ||
                                        !string.Equals(verifiedEmail, employee.Email, StringComparison.OrdinalIgnoreCase))
                                    {
                                        throw new InvalidOperationException("User update verification failed: data mismatch after update.");
                                    }
                                }
                                else
                                {
                                    throw new InvalidOperationException("User update verification failed: user not found after update.");
                                }
                            }
                        }
                        Debug.WriteLine($"User account updated for Employee ID: {employee.EmployeeID}");
                    }

                    // 3. Check and Update TUNG.EMPLOYEES
                    bool needsEmployeeUpdate = false;
                    if (!string.Equals(currentEmployee.FirstName, employee.FirstName, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.LastName, employee.LastName, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.Title, employee.Title, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.TitleOfCourtesy, employee.TitleOfCourtesy, StringComparison.OrdinalIgnoreCase) ||
                        !Nullable.Equals(currentEmployee.BirthDate, employee.BirthDate) ||
                        !Nullable.Equals(currentEmployee.HireDate, employee.HireDate) ||
                        !string.Equals(currentEmployee.Address, employee.Address, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.City, employee.City, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.Region, employee.Region, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.PostalCode, employee.PostalCode, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.Country, employee.Country, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.HomePhone, employee.HomePhone, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(currentEmployee.Extension, employee.Extension, StringComparison.OrdinalIgnoreCase) ||
                        !Nullable.Equals(currentEmployee.ReportsTo, employee.ReportsTo)
                        )
                    {
                        needsEmployeeUpdate = true;
                    }

                    if (needsEmployeeUpdate)
                    {
                        string updateEmployeeQuery = @"UPDATE TUNG.EMPLOYEES 
                                                        SET LASTNAME = :LastName, FIRSTNAME = :FirstName, TITLE = :Title, TITLEOFCOURTESY = :TitleOfCourtesy, 
                                                            BIRTHDATE = :BirthDate, HIREDATE = :HireDate, ADDRESS = :Address, CITY = :City, REGION = :Region, 
                                                            POSTALCODE = :PostalCode, COUNTRY = :Country, HOMEPHONE = :HomePhone, EXTENSION = :Extension, 
                                                            REPORTSTO = :ReportsTo 
                                                        WHERE EMPLOYEEID = :EmployeeID";
                        using (OracleCommand updateCmd = new OracleCommand())
                        {
                            updateCmd.Connection = connection;
                            updateCmd.Transaction = transaction;
                            updateCmd.CommandText = updateEmployeeQuery;
                            updateCmd.Parameters.Add(new OracleParameter("LastName", employee.LastName));
                            updateCmd.Parameters.Add(new OracleParameter("FirstName", employee.FirstName));
                            updateCmd.Parameters.Add(new OracleParameter("Title", employee.Title));
                            updateCmd.Parameters.Add(new OracleParameter("TitleOfCourtesy", employee.TitleOfCourtesy));
                            updateCmd.Parameters.Add(new OracleParameter("BirthDate", employee.BirthDate));
                            updateCmd.Parameters.Add(new OracleParameter("HireDate", employee.HireDate));
                            updateCmd.Parameters.Add(new OracleParameter("Address", employee.Address));
                            updateCmd.Parameters.Add(new OracleParameter("City", employee.City));
                            updateCmd.Parameters.Add(new OracleParameter("Region", employee.Region));
                            updateCmd.Parameters.Add(new OracleParameter("PostalCode", employee.PostalCode));
                            updateCmd.Parameters.Add(new OracleParameter("Country", employee.Country));
                            updateCmd.Parameters.Add(new OracleParameter("HomePhone", employee.HomePhone));
                            updateCmd.Parameters.Add(new OracleParameter("Extension", employee.Extension));
                            updateCmd.Parameters.Add(new OracleParameter("ReportsTo", employee.ReportsTo));
                            updateCmd.Parameters.Add(new OracleParameter("EmployeeID", employee.EmployeeID));
                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new InvalidOperationException("Failed to update employee record or employee not found during update.");
                            }
                        }
                        Debug.WriteLine($"Employee record updated for Employee ID: {employee.EmployeeID}");
                    }

                    // Final verification after both updates (if applicable)
                    string finalVerifyQuery = @"SELECT 
                                                e.EMPLOYEEID, e.LASTNAME, e.FIRSTNAME, e.TITLE, e.TITLEOFCOURTESY, e.BIRTHDATE, e.HIREDATE, e.ADDRESS, e.CITY, e.REGION, e.POSTALCODE, e.COUNTRY, e.HOMEPHONE, e.EXTENSION, e.REPORTSTO, 
                                                u.USERID, u.ROLEID, u.USERNAME, u.PASSWORDHASH, u.EMAIL AS EMAIL_U, u.ISACTIVE, u.CREATEDAT AS CREATEDAT_U, u.UPDATEDAT AS UPDATEDAT_U,
                                                r.ROLENAME
                                                FROM TUNG.EMPLOYEES e
                                                LEFT JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                                                LEFT JOIN TUNG.ROLES r ON u.ROLEID = r.ROLEID
                                                WHERE e.EMPLOYEEID = :EmployeeID";
                    using (OracleCommand verifyCmd = new OracleCommand())
                    {
                        verifyCmd.Connection = connection;
                        verifyCmd.Transaction = transaction;
                        verifyCmd.CommandText = finalVerifyQuery;
                        verifyCmd.Parameters.Add(new OracleParameter("EmployeeID", employee.EmployeeID));
                        using (OracleDataReader verifyReader = verifyCmd.ExecuteReader())
                        {
                            if (verifyReader.Read())
                            {
                                Employee verifiedEmployee = MapEmployeeFromReader(verifyReader);
                                // Compare the original `employee` object with `verifiedEmployee`
                                // This is a simplified comparison, in a real app, you'd compare all relevant fields
                                if (verifiedEmployee.FirstName != employee.FirstName ||
                                    verifiedEmployee.LastName != employee.LastName ||
                                    verifiedEmployee.IsActive != employee.IsActive ||
                                    verifiedEmployee.RoleID != employee.RoleID ||
                                    !string.Equals(verifiedEmployee.Email, employee.Email, StringComparison.OrdinalIgnoreCase))
                                {
                                    throw new InvalidOperationException("Final verification failed: data mismatch after update.");
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("Final verification failed: employee not found after update.");
                            }
                        }
                    }

                    transaction.Commit();
                    success = true;
                    Debug.WriteLine($"Successfully updated employee with ID: {employee.EmployeeID}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine($"Error in UpdateEmployee: {ex.Message}");
                    throw;
                }
            }
            return success;
        }
        #endregion

        #region Control Employee Status
        public bool ToggleEmployeeStatus(int employeeId, bool isActive)
        {
            bool success = false;
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                OracleTransaction transaction = connection.BeginTransaction();
                try
                {
                    // 1. Verify current USERID, ROLEID, ISACTIVE for the employee
                    string selectUserQuery = @"SELECT USERID, ROLEID, ISACTIVE 
                                                FROM TUNG.USERS 
                                                WHERE EMPLOYEEID = :EmployeeID FOR UPDATE";
                    
                    int currentUserId = 0;
                    int currentRoleId = 0;
                    bool currentIsActive = false;

                    using (OracleCommand command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = selectUserQuery;
                        command.Parameters.Add(new OracleParameter("EmployeeID", employeeId));
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentUserId = reader.GetInt32(reader.GetOrdinal("USERID"));
                                currentRoleId = reader.GetInt32(reader.GetOrdinal("ROLEID"));
                                currentIsActive = Convert.ToBoolean(reader["ISACTIVE"]);
                            }
                            else
                            {
                                throw new InvalidOperationException($"User account for Employee ID {employeeId} not found.");
                            }
                        }
                    }

                    // 2. Check for last active administrator
                    if (!isActive && currentRoleId == 1) // Trying to deactivate an admin
                    {
                        string countAdminQuery = @"SELECT COUNT(*) FROM TUNG.USERS WHERE ROLEID = 1 AND ISACTIVE = 1";
                        using (OracleCommand countCmd = new OracleCommand())
                        {
                            countCmd.Connection = connection;
                            countCmd.Transaction = transaction;
                            countCmd.CommandText = countAdminQuery;
                            int activeAdmins = Convert.ToInt32(countCmd.ExecuteScalar());
                            if (activeAdmins <= 1)
                            {
                                throw new InvalidOperationException("Cannot deactivate the last active administrator.");
                            }
                        }
                    }

                    // 3. Update TUNG.USERS
                    string updateUserStatusQuery = @"UPDATE TUNG.USERS 
                                                    SET ISACTIVE = :IsActive, UPDATEDAT = SYSDATE 
                                                    WHERE USERID = :UserId AND EMPLOYEEID = :EmployeeID AND ROLEID = :RoleID";
                    using (OracleCommand updateCmd = new OracleCommand())
                    {
                        updateCmd.Connection = connection;
                        updateCmd.Transaction = transaction;
                        updateCmd.CommandText = updateUserStatusQuery;
                        updateCmd.Parameters.Add(new OracleParameter("IsActive", isActive ? 1 : 0));
                        updateCmd.Parameters.Add(new OracleParameter("UserId", currentUserId));
                        updateCmd.Parameters.Add(new OracleParameter("EmployeeID", employeeId));
                        updateCmd.Parameters.Add(new OracleParameter("RoleID", currentRoleId));
                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("Failed to update user status or user not found during update.");
                        }
                    }

                    // 4. Verify update
                    string verifyStatusQuery = @"SELECT ISACTIVE FROM TUNG.USERS WHERE USERID = :UserId";
                    using (OracleCommand verifyCmd = new OracleCommand())
                    {
                        verifyCmd.Connection = connection;
                        verifyCmd.Transaction = transaction;
                        verifyCmd.CommandText = verifyStatusQuery;
                        verifyCmd.Parameters.Add(new OracleParameter("UserId", currentUserId));
                        using (OracleDataReader verifyReader = verifyCmd.ExecuteReader())
                        {
                            if (verifyReader.Read())
                            {
                                bool verifiedIsActive = Convert.ToBoolean(verifyReader["ISACTIVE"]);
                                if (verifiedIsActive != isActive)
                                {
                                    throw new InvalidOperationException("Status update verification failed: data mismatch after update.");
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("Status update verification failed: user not found after update.");
                            }
                        }
                    }

                    transaction.Commit();
                    success = true;
                    Debug.WriteLine($"Successfully toggled employee status for Employee ID: {employeeId} to isActive: {isActive}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine($"Error in ToggleEmployeeStatus: {ex.Message}");
                    throw;
                }
            }
            return success;
        }
        #endregion
    }
}