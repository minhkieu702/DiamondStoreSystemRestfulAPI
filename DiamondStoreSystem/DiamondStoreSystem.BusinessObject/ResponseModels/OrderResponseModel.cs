using DiamondStoreSystem.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResponseModels
{
    public class OrderResponseModel
    {
        public string OrderID { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime? DateReceived { get; set; }
        public double TotalPrice { get; set; }
        public string CustomerID { get; set; }
        public AccountResponseModel Customer { get; set; }
        public AccountResponseModel Employee { get; set; }
        public ICollection<ProductResponseModel> Products { get; set; }
        public string PayMethod { get; set; }
        public string EmployeeAssignID { get; set; }
    }
}
