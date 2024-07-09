using DiamondStoreSystem.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResquestModels
{
    public class OrderRequestModel
    {
        public string OrderID { get; set; }
        public OrderStatus OrderStatus { get; set; }=OrderStatus.Pending;
        public DateTime DateOrdered { get; set; }
        public DateTime? DateReceived { get; set; }
        public double TotalPrice { get; set; }
        public string CustomerID { get; set; }
        public string EmployeeAssignID { get; set; }
        public PayMethod PayMethod { get; set; }
        [JsonIgnore]
        public bool? Block { get; set; } = false;
    }
}
