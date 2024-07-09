using DiamondStoreSystem.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResquestModels
{
    public class CartRequestModel
    {
        public List<ProductTempRequestModel> Cart { get; set; }
    }
    public class ProductTempRequestModel
    {
        public string? AccessoryID { get; set; }
        public string MainDiamondID { get; set; }
        public List<string> SubDiamondID { get; set; }
    }
}
