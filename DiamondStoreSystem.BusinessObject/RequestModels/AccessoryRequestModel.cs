using DiamondStoreSystem.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResquestModels
{
    public class AccessoryRequestModel
    {
        public string AccessoryID { get; set; }
        public string AccessoryName { get; set; }
        public string Description { get; set; }
        public Material Material { get; set; }
        public Style Style { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public int UnitInStock { get; set; }
        public string SKU { get; set; }
        [JsonIgnore]
        public bool Block { get; set; } = false;
    }
}
