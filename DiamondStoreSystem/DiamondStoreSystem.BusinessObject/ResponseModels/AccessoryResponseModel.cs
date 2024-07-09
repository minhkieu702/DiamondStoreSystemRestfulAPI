using DiamondStoreSystem.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResponseModels
{
    public class AccessoryResponseModel
    {
        public string AccessoryID { get; set; }
        public string AccessoryName { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string Style { get; set; }
        public string Brand { get; set; }
        public bool Block { get; set; }
        public double Price { get; set; }
        public int UnitInStock { get; set; }
        public string SKU { get; set; }
    }
}
