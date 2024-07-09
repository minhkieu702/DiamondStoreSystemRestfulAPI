using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class SubDiamond
    {
        [Key, ForeignKey("Diamond")]
        public string SubDiamondID { get; set; }
        public string ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        public Diamond Diamond { get; set; }
    }
}
