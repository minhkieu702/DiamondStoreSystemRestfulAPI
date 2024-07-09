using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class Product
    {
        [Key]
        public string ProductID { get; set; }
        public double Price { get; set; }
        public bool Block { get; set; }
        public string? AccessoryID { get; set; }
        public string OrderID { get; set; }
        public string MainDiamondID { get; set; }

        [ForeignKey("AccessoryID")]
        public Accessory? Accessory { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        [ForeignKey("MainDiamondID")]
        public Diamond MainDiamond { get; set; }
        public Warranty? Warranty { get; set; }
        public ICollection<SubDiamond> SubDiamonds { get; set; }
    }
}
