using DiamondStoreSystem.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResponseModels
{
    public class SubDiamondResponseModel
    {
        public string SubDiamondID { get; set; }
        public string ProductID { get; set; }
        public DiamondResponseModel SubDiamond { get; set; }
        public ProductResponseModel Product {  get; set; }
    }
}
