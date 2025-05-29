using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shopping.Model1
{
    public class User
    {
        public int UserID { get; set; }
        [ForeignKey("Role")]
        public int RoleID { get; set; } // Foreign key to Role table
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("EmployeeID")]
        public int EmployeeID { get; set; } // Foreign key to Employee table
        public Employee Employee { get; set; }
        // Additional properties can be added as needed
    }
}