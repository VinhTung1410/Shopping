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
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Starting UpdateEmployee for ID: {employee.EmployeeID}");
                    
                    using (OracleTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // First verify if employee exists and get current data
                            string verifyQuery = @"
                                SELECT e.EMPLOYEEID, e.FIRSTNAME, e.LASTNAME,
                                       u.USERID, u.ROLEID, u.ISACTIVE, u.EMAIL 
                                FROM TUNG.EMPLOYEES e
                                JOIN TUNG.USERS u ON e.EMPLOYEEID = u.EMPLOYEEID
                                WHERE e.EMPLOYEEID = :EmployeeID
                                FOR UPDATE";

                            int? userId = null;
                            int? currentRoleId = null;
                            int? currentIsActive = null;
                            string currentEmail = null;
                            string currentFirstName = null;
                            string currentLastName = null;

                            using (OracleCommand verifyCmd = new OracleCommand(verifyQuery, conn))
                            {
                                verifyCmd.Transaction = transaction;
                                verifyCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employee.EmployeeID;

                                using (var reader = verifyCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        userId = Convert.ToInt32(reader["USERID"]);
                                        currentRoleId = Convert.ToInt32(reader["ROLEID"]);
                                        currentIsActive = Convert.ToInt32(reader["ISACTIVE"]);
                                        currentEmail = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : null;
                                        currentFirstName = reader["FIRSTNAME"].ToString();
                                        currentLastName = reader["LASTNAME"].ToString();

                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Found existing record:");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] UserID: {userId}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Current Name: {currentFirstName} {currentLastName}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Current RoleID: {currentRoleId}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Current IsActive: {currentIsActive}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Current Email: {currentEmail}");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("[DEBUG] Employee not found in database");
                                        throw new Exception($"Employee ID {employee.EmployeeID} not found.");
                                    }
                                }
                            }

                            // Check if any changes are needed for USERS table
                            bool needsUserUpdate = false;
                            if (currentRoleId != employee.RoleID ||
                                currentIsActive != (employee.IsActive ? 1 : 0) ||
                                ((currentEmail ?? "") != (employee.Email ?? "")))
                            {
                                needsUserUpdate = true;
                                System.Diagnostics.Debug.WriteLine("[DEBUG] User update needed:");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] Role change: {currentRoleId} -> {employee.RoleID}");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] IsActive change: {currentIsActive} -> {(employee.IsActive ? 1 : 0)}");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] Email change: {currentEmail} -> {employee.Email}");
                            }

                            // Update USERS table if needed
                            if (needsUserUpdate)
                            {
                                // First check if we can update this user (e.g., not the last admin)
                                if (!employee.IsActive && employee.RoleID == 1)
                                {
                                    string checkAdminQuery = @"
                                        SELECT COUNT(*) 
                                        FROM TUNG.USERS 
                                        WHERE ROLEID = 1 
                                        AND ISACTIVE = 1 
                                        AND USERID != :UserID";

                                    using (OracleCommand checkCmd = new OracleCommand(checkAdminQuery, conn))
                                    {
                                        checkCmd.Transaction = transaction;
                                        checkCmd.Parameters.Add(":UserID", OracleDbType.Decimal).Value = userId.Value;

                                        int activeAdminCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Active admin count: {activeAdminCount}");
                                        
                                        if (activeAdminCount == 0)
                                        {
                                            throw new Exception("Cannot deactivate the last active administrator.");
                                        }
                                    }
                                }

                                // Check if the role exists
                                string checkRoleQuery = "SELECT COUNT(*) FROM TUNG.ROLES WHERE ROLEID = :RoleID";
                                using (OracleCommand checkRoleCmd = new OracleCommand(checkRoleQuery, conn))
                                {
                                    checkRoleCmd.Transaction = transaction;
                                    checkRoleCmd.Parameters.Add(":RoleID", OracleDbType.Decimal).Value = employee.RoleID;

                                    int roleCount = Convert.ToInt32(checkRoleCmd.ExecuteScalar());
                                    if (roleCount == 0)
                                    {
                                        throw new Exception($"Role ID {employee.RoleID} does not exist.");
                                    }
                                }

                                // Get current values before update
                                string getCurrentQuery = @"
                                    SELECT ROLEID, ISACTIVE, EMAIL 
                                    FROM TUNG.USERS 
                                    WHERE USERID = :UserID 
                                    FOR UPDATE";

                                int oldRoleId = 0;
                                int oldIsActive = 0;
                                string oldEmail = null;

                                using (OracleCommand getCurrentCmd = new OracleCommand(getCurrentQuery, conn))
                                {
                                    getCurrentCmd.Transaction = transaction;
                                    getCurrentCmd.Parameters.Add(":UserID", OracleDbType.Decimal).Value = userId.Value;

                                    using (var reader = getCurrentCmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            oldRoleId = Convert.ToInt32(reader["ROLEID"]);
                                            oldIsActive = Convert.ToInt32(reader["ISACTIVE"]);
                                            oldEmail = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : null;

                                            System.Diagnostics.Debug.WriteLine("[DEBUG] Current values before update:");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] RoleID: {oldRoleId} -> {employee.RoleID}");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] IsActive: {oldIsActive} -> {(employee.IsActive ? 1 : 0)}");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Email: {oldEmail} -> {employee.Email}");
                                        }
                                    }
                                }

                                // Only update if there are actual changes
                                if (oldRoleId != employee.RoleID ||
                                    oldIsActive != (employee.IsActive ? 1 : 0) ||
                                    ((oldEmail ?? "") != (employee.Email ?? "")))
                                {
                                    string userQuery = @"
                                        UPDATE TUNG.USERS 
                                        SET EMAIL = :Email,
                                            ROLEID = :RoleID,
                                            ISACTIVE = :IsActive,
                                            UPDATEDAT = SYSDATE
                                        WHERE USERID = :UserID";

                                    using (OracleCommand userCmd = new OracleCommand(userQuery, conn))
                                    {
                                        userCmd.Transaction = transaction;

                                        // Debug the incoming values
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Incoming values for update:");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] UserID type: {userId.Value.GetType()}, Value: {userId.Value}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] RoleID type: {employee.RoleID.GetType()}, Value: {employee.RoleID}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] IsActive type: {employee.IsActive.GetType()}, Value: {employee.IsActive}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Email type: {(employee.Email != null ? employee.Email.GetType().ToString() : "null")}, Value: {employee.Email ?? "null"}");

                                        // Add parameters with correct Oracle types matching the schema
                                        var paramUserId = new OracleParameter
                                        {
                                            ParameterName = ":UserID",
                                            OracleDbType = OracleDbType.Int32,  // NUMBER without precision
                                            Value = userId.Value
                                        };
                                        userCmd.Parameters.Add(paramUserId);

                                        var paramEmail = new OracleParameter
                                        {
                                            ParameterName = ":Email",
                                            OracleDbType = OracleDbType.Varchar2,
                                            Size = 255,
                                            Value = (object)employee.Email ?? DBNull.Value
                                        };
                                        userCmd.Parameters.Add(paramEmail);

                                        var paramRoleId = new OracleParameter
                                        {
                                            ParameterName = ":RoleID",
                                            OracleDbType = OracleDbType.Int32,  // NUMBER without precision
                                            Value = employee.RoleID
                                        };
                                        userCmd.Parameters.Add(paramRoleId);

                                        var paramIsActive = new OracleParameter
                                        {
                                            ParameterName = ":IsActive",
                                            OracleDbType = OracleDbType.Int32,  // NUMBER(1,0)
                                            Value = employee.IsActive ? 1 : 0
                                        };
                                        userCmd.Parameters.Add(paramIsActive);

                                        // Debug the parameter values before execution
                                        foreach (OracleParameter param in userCmd.Parameters)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Parameter {param.ParameterName}:");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG]   Type: {param.OracleDbType}");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG]   Value: {(param.Value == DBNull.Value ? "NULL" : param.Value.ToString())}");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG]   .NET Type: {(param.Value == DBNull.Value ? "DBNull" : param.Value.GetType().ToString())}");
                                        }

                                        try
                                        {
                                            int userResult = userCmd.ExecuteNonQuery();
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Users update affected {userResult} rows");

                                            if (userResult <= 0)
                                            {
                                                throw new Exception("Failed to update user information in USERS table.");
                                            }
                                        }
                                        catch (OracleException oex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error updating USERS: {oex.Message}");
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error code: {oex.Number}");
                                            throw;
                                        }
                                    }

                                    // Verify the update immediately
                                    string verifyQuery1 = @"
                                        SELECT ROLEID, ISACTIVE, EMAIL
                                        FROM TUNG.USERS 
                                        WHERE USERID = :UserID";

                                    using (OracleCommand verifyCmd = new OracleCommand(verifyQuery1, conn))
                                    {
                                        verifyCmd.Transaction = transaction;
                                        verifyCmd.Parameters.Add(":UserID", OracleDbType.Decimal).Value = userId.Value;

                                        using (var reader = verifyCmd.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                int newRoleId = Convert.ToInt32(reader["ROLEID"]);
                                                int newIsActive = Convert.ToInt32(reader["ISACTIVE"]);
                                                string newEmail = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : null;

                                                System.Diagnostics.Debug.WriteLine("[DEBUG] Values after update:");
                                                System.Diagnostics.Debug.WriteLine($"[DEBUG] RoleID: {newRoleId} (Expected: {employee.RoleID})");
                                                System.Diagnostics.Debug.WriteLine($"[DEBUG] IsActive: {newIsActive} (Expected: {(employee.IsActive ? 1 : 0)})");
                                                System.Diagnostics.Debug.WriteLine($"[DEBUG] Email: {newEmail} (Expected: {employee.Email ?? "NULL"})");

                                                if (newRoleId != employee.RoleID ||
                                                    newIsActive != (employee.IsActive ? 1 : 0) ||
                                                    ((newEmail ?? "") != (employee.Email ?? "")))
                                                {
                                                    System.Diagnostics.Debug.WriteLine("[DEBUG] Update verification failed - rolling back transaction");
                                                    transaction.Rollback();
                                                    throw new Exception("Update verification failed");
                                                }
                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine("[DEBUG] User record not found after update - rolling back transaction");
                                                transaction.Rollback();
                                                throw new Exception("Failed to verify update - user record not found.");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("[DEBUG] No changes needed for USERS table");
                                }
                            }

                            // Check if any changes are needed for EMPLOYEES table
                            bool needsEmployeeUpdate = false;
                            if ((currentFirstName != employee.FirstName) ||
                                (currentLastName != employee.LastName))
                            {
                                needsEmployeeUpdate = true;
                                System.Diagnostics.Debug.WriteLine("[DEBUG] Employee update needed:");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] Name change: {currentFirstName} {currentLastName} -> {employee.FirstName} {employee.LastName}");
                            }

                            // Update EMPLOYEES table if needed
                            if (needsEmployeeUpdate)
                            {
                                string empQuery = @"
                                    UPDATE TUNG.EMPLOYEES 
                                    SET FIRSTNAME = :FirstName,
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

                                    var parameters = new OracleParameter[]
                                    {
                                        new OracleParameter(":EmployeeID", OracleDbType.Int32) { Value = employee.EmployeeID },
                                        new OracleParameter(":FirstName", OracleDbType.Varchar2, 255) { Value = (object)employee.FirstName ?? DBNull.Value },
                                        new OracleParameter(":LastName", OracleDbType.Varchar2, 255) { Value = (object)employee.LastName ?? DBNull.Value },
                                        new OracleParameter(":Title", OracleDbType.Varchar2, 255) { Value = (object)employee.Title ?? DBNull.Value },
                                        new OracleParameter(":TitleOfCourtesy", OracleDbType.Varchar2, 255) { Value = (object)employee.TitleOfCourtesy ?? DBNull.Value },
                                        new OracleParameter(":BirthDate", OracleDbType.Date) { Value = (object)employee.BirthDate ?? DBNull.Value },
                                        new OracleParameter(":HireDate", OracleDbType.Date) { Value = (object)employee.HireDate ?? DBNull.Value },
                                        new OracleParameter(":Address", OracleDbType.Varchar2, 255) { Value = (object)employee.Address ?? DBNull.Value },
                                        new OracleParameter(":City", OracleDbType.Varchar2, 255) { Value = (object)employee.City ?? DBNull.Value },
                                        new OracleParameter(":Region", OracleDbType.Varchar2, 255) { Value = (object)employee.Region ?? DBNull.Value },
                                        new OracleParameter(":PostalCode", OracleDbType.Varchar2, 255) { Value = (object)employee.PostalCode ?? DBNull.Value },
                                        new OracleParameter(":Country", OracleDbType.Varchar2, 255) { Value = (object)employee.Country ?? DBNull.Value },
                                        new OracleParameter(":HomePhone", OracleDbType.Varchar2, 255) { Value = (object)employee.HomePhone ?? DBNull.Value },
                                        new OracleParameter(":Extension", OracleDbType.Varchar2, 255) { Value = (object)employee.Extension ?? DBNull.Value },
                                        new OracleParameter(":ReportsTo", OracleDbType.Int32) { Value = (object)employee.ReportsTo ?? DBNull.Value }
                                    };

                                    empCmd.Parameters.AddRange(parameters);

                                    foreach (OracleParameter param in parameters)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Parameter {param.ParameterName}: {(param.Value == DBNull.Value ? "NULL" : param.Value.ToString())}");
                                    }

                                    try
                                    {
                                        int empResult = empCmd.ExecuteNonQuery();
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Employees update affected {empResult} rows");

                                        if (empResult <= 0)
                                        {
                                            throw new Exception("Failed to update employee information in EMPLOYEES table.");
                                        }
                                    }
                                    catch (OracleException oex)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error updating EMPLOYEES: {oex.Message}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error code: {oex.Number}");
                                        throw;
                                    }
                                }
                            }

                            // If we got here, either no updates were needed or they were successful
                            // Verify final state
                            string verifyUpdateQuery = @"
                                SELECT u.ISACTIVE, u.ROLEID, u.EMAIL, e.FIRSTNAME, e.LASTNAME
                                FROM TUNG.USERS u
                                JOIN TUNG.EMPLOYEES e ON e.EMPLOYEEID = u.EMPLOYEEID
                                WHERE u.USERID = :UserID";

                            using (OracleCommand verifyCmd = new OracleCommand(verifyUpdateQuery, conn))
                            {
                                verifyCmd.Transaction = transaction;
                                verifyCmd.Parameters.Add(":UserID", OracleDbType.Int32).Value = userId;

                                using (var reader = verifyCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int finalIsActive = Convert.ToInt32(reader["ISACTIVE"]);
                                        int finalRoleId = Convert.ToInt32(reader["ROLEID"]);
                                        string finalFirstName = reader["FIRSTNAME"].ToString();
                                        string finalLastName = reader["LASTNAME"].ToString();
                                        string finalEmail = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : null;
                                        
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Final verification results:");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] IsActive: {finalIsActive} (Expected: {(employee.IsActive ? 1 : 0)})");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] RoleID: {finalRoleId} (Expected: {employee.RoleID})");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Name: {finalFirstName} {finalLastName}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Email: {finalEmail} (Expected: {employee.Email ?? "NULL"})");

                                        bool isActiveMatch = finalIsActive == (employee.IsActive ? 1 : 0);
                                        bool roleMatch = finalRoleId == employee.RoleID;
                                        bool nameMatch = finalFirstName == employee.FirstName && finalLastName == employee.LastName;
                                        bool emailMatch = (finalEmail ?? "NULL") == (employee.Email ?? "NULL");

                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Matches - IsActive: {isActiveMatch}, Role: {roleMatch}, Name: {nameMatch}, Email: {emailMatch}");

                                        if (isActiveMatch && roleMatch && nameMatch && emailMatch)
                                        {
                                            System.Diagnostics.Debug.WriteLine("[DEBUG] Update verified successfully");
                                            transaction.Commit();
                                            return true;
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine("[DEBUG] Update verification failed - values do not match expected results");
                                            throw new Exception("Update verification failed - values do not match expected results.");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Failed to verify update - user record not found after update.");
                                    }
                                }
                            }
                        }
                        catch (OracleException oex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error in UpdateEmployee: {oex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error code: {oex.Number}");
                            transaction.Rollback();
                            throw new Exception($"Database error: {oex.Message}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Error in UpdateEmployee transaction: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Error in UpdateEmployee: {ex.Message}");
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
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Connection opened for employee {employeeId}");
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Attempting to set IsActive to: {isActive}");
                    
                    using (OracleTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // First verify the exact record we're trying to update
                            string verifyRecordQuery = @"
                                SELECT u.USERID, u.ROLEID, u.ISACTIVE, u.EMPLOYEEID,
                                       e.FIRSTNAME, e.LASTNAME
                                FROM TUNG.USERS u
                                JOIN TUNG.EMPLOYEES e ON e.EMPLOYEEID = u.EMPLOYEEID
                                WHERE u.EMPLOYEEID = :EmployeeID";

                            int? userId = null;
                            int? currentRoleId = null;
                            int? currentIsActive = null;
                            string firstName = "", lastName = "";

                            using (OracleCommand verifyCmd = new OracleCommand(verifyRecordQuery, conn))
                            {
                                verifyCmd.Transaction = transaction;
                                verifyCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employeeId;

                                using (var reader = verifyCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        userId = Convert.ToInt32(reader["USERID"]);
                                        currentRoleId = Convert.ToInt32(reader["ROLEID"]);
                                        currentIsActive = reader["ISACTIVE"] != DBNull.Value ? Convert.ToInt32(reader["ISACTIVE"]) : 0;
                                        firstName = reader["FIRSTNAME"].ToString();
                                        lastName = reader["LASTNAME"].ToString();

                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Found user record:");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] UserID: {userId}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] EmployeeID: {employeeId}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Name: {firstName} {lastName}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Current IsActive: {currentIsActive}");
                                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Current RoleID: {currentRoleId}");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("[DEBUG] User record not found");
                                        throw new Exception($"Employee ID {employeeId} not found in USERS table.");
                                    }
                                }
                            }

                            // Check for last active admin
                            if (!isActive && currentRoleId == 1)
                            {
                                string checkAdminQuery = @"
                                    SELECT COUNT(*) 
                                    FROM TUNG.USERS 
                                    WHERE ROLEID = 1 
                                    AND ISACTIVE = 1 
                                    AND EMPLOYEEID != :EmployeeID";

                                using (OracleCommand checkCmd = new OracleCommand(checkAdminQuery, conn))
                                {
                                    checkCmd.Transaction = transaction;
                                    checkCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employeeId;

                                    int activeAdminCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Active admin count: {activeAdminCount}");
                                    
                                    if (activeAdminCount == 0)
                                    {
                                        throw new Exception("Cannot deactivate the last active administrator.");
                                    }
                                }
                            }

                            // Update with exact column types
                            string updateQuery = @"
                                UPDATE TUNG.USERS 
                                SET ISACTIVE = :IsActive,
                                    UPDATEDAT = SYSDATE
                                WHERE USERID = :UserID 
                                AND EMPLOYEEID = :EmployeeID 
                                AND ROLEID = :RoleID";

                            using (OracleCommand cmd = new OracleCommand(updateQuery, conn))
                            {
                                cmd.Transaction = transaction;
                                
                                // Add parameters with exact Oracle types
                                OracleParameter paramIsActive = new OracleParameter(":IsActive", OracleDbType.Int32);
                                paramIsActive.Value = isActive ? 1 : 0;
                                cmd.Parameters.Add(paramIsActive);

                                OracleParameter paramUserId = new OracleParameter(":UserID", OracleDbType.Int32);
                                paramUserId.Value = userId.Value;
                                cmd.Parameters.Add(paramUserId);

                                OracleParameter paramEmployeeId = new OracleParameter(":EmployeeID", OracleDbType.Int32);
                                paramEmployeeId.Value = employeeId;
                                cmd.Parameters.Add(paramEmployeeId);

                                OracleParameter paramRoleId = new OracleParameter(":RoleID", OracleDbType.Int32);
                                paramRoleId.Value = currentRoleId.Value;
                                cmd.Parameters.Add(paramRoleId);

                                System.Diagnostics.Debug.WriteLine("[DEBUG] Executing update with parameters:");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] UserID: {userId}");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] EmployeeID: {employeeId}");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] RoleID: {currentRoleId}");
                                System.Diagnostics.Debug.WriteLine($"[DEBUG] New IsActive: {(isActive ? 1 : 0)}");

                                try
                                {
                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Rows affected by update: {rowsAffected}");

                                    if (rowsAffected > 0)
                                    {
                                        // Verify the update
                                        string verifyUpdateQuery = @"
                                            SELECT ISACTIVE 
                                            FROM TUNG.USERS 
                                            WHERE USERID = :UserID 
                                            AND EMPLOYEEID = :EmployeeID";

                                        using (OracleCommand verifyCmd = new OracleCommand(verifyUpdateQuery, conn))
                                        {
                                            verifyCmd.Transaction = transaction;
                                            verifyCmd.Parameters.Add(":UserID", OracleDbType.Int32).Value = userId;
                                            verifyCmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employeeId;

                                            var verifyResult = verifyCmd.ExecuteScalar();
                                            int newIsActive = Convert.ToInt32(verifyResult);
                                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Verify result - New IsActive value: {newIsActive}");

                                            if ((newIsActive == 1) == isActive)
                                            {
                                                System.Diagnostics.Debug.WriteLine("[DEBUG] Update verified - committing transaction");
                                                transaction.Commit();
                                                return true;
                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine("[DEBUG] Update verification failed - rolling back");
                                                transaction.Rollback();
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("[DEBUG] No rows updated - rolling back transaction");
                                        transaction.Rollback();
                                        return false;
                                    }
                                }
                                catch (OracleException oex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error during update: {oex.Message}");
                                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Oracle error code: {oex.Number}");
                                    throw;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Error in transaction: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Error in ToggleEmployeeStatus: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
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