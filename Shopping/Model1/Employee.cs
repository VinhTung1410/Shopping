using System;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Model1
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public int? UserID { get; set; }
        public int RoleID { get; set; } = 2; // Default to User role (2)

        [Required(ErrorMessage = "Username is required")]
        [StringLength(255)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string Region { get; set; }

        [StringLength(255)]
        public string PostalCode { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string HomePhone { get; set; }

        [StringLength(255)]
        public string Extension { get; set; }

        public int? ReportsTo { get; set; }

        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}