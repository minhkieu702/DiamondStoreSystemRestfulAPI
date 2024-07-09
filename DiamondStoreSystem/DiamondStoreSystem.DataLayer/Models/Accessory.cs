using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class Accessory
    {
        [Key]
        public string AccessoryID { get; set; }
        public string AccessoryName { get; set; }
        public string Description { get; set; }
        public int Material { get; set; }
        public int Style { get; set; }
        public string Brand { get; set; }
        public bool Block { get; set; }
        public double Price { get; set; }
        public int UnitInStock { get; set; }
        public string SKU { get; set; }
        public string? ImageUrl {  get; set; }
    }
}
