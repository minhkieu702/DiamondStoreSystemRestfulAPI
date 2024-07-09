using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResquestModels
{
    public class ProductRequestModel
    {
        public string MainDiamondID {  get; set; }
        public string ProductID { get; set; }
        public double Price { get; set; }
        public string? AccessoryID { get; set; }
        public string OrderID { get; set; }
    }
}
