using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class Account
    {
        [Key]
        public string AccountID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public DateTime JoinDate { get; set; }
        public int? LoyaltyPoint { get; set; }
        public bool Block { get; set; }
        public int Role { get; set; }
        public int? WorkingSchedule { get; set; }
        public ICollection<Order>? OrdersCustomer { get; set; }
        public ICollection<Order>? OrdersStaff { get; set; }
    }
}
