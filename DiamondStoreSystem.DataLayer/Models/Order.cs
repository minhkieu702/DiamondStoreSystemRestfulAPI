using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class Order
    {
        [Key]
        public string OrderID { get; set; }
        public int OrderStatus { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime? DateReceived { get; set; }
        public double TotalPrice { get; set; }
        public string CustomerID { get; set; }
        public string EmployeeAssignID { get; set; }
        public int PayMethod { get; set; }
        public bool Block { get; set; }

        [ForeignKey("CustomerID")]
        public Account Customer { get; set; }

        [ForeignKey("EmployeeAssignID")]
        public Account EmployeeAccount { get; set; }
        public VnPaymentResponse? VnPaymentResponse { get; set; }
        public ICollection<Product> Products { get; set; }
    }

}
