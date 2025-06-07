using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shopping.Model1
{
    public class Order
    {
        [Key]
        [Column("ORDERID")]
        public int OrderID { get; set; }

        [ForeignKey("Employee")]
        [Column("EMPLOYEEID")]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        [Column("ORDERDATE")]
        public DateTime OrderDate { get; set; }

        [Column("REQUIREDDATE")]
        public DateTime? RequiredDate { get; set; }

        [Column("SHIPNAME")]
        [StringLength(255)]
        public string ShipName { get; set; }

        [Column("SHIPADDRESS")]
        [StringLength(255)]
        public string ShipAddress { get; set; }

        // Navigation property for OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            OrderDate = DateTime.Now; // Set default order date to now
        }
    }
    public class OrderDetail
    {
        // Composite Primary Key
        [Key]
        [Column("ORDERID", Order = 1)]
        public int OrderID { get; set; }

        [Key]
        [Column("PRODUCTID", Order = 2)]
        public int ProductID { get; set; }

        // Other properties
        [Column("UNITPRICE", TypeName = "decimal")]
        public decimal UnitPrice { get; set; }

        [Column("QUANTITY")]
        public int Quantity { get; set; }

        [Column("DISCOUNT")]
        public double Discount { get; set; }

        [Column("NOTE", TypeName = "nclob")]
        public string Note { get; set; }

        // Navigation properties for relationships
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        // Default constructor if needed
        public OrderDetail()
        {

        }
    }
}