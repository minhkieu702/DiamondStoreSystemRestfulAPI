using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class Diamond
    {
        [Key]
        public string DiamondID { get; set; }
        public string Origin { get; set; }
        public int LabCreated { get; set; }
        public double TablePercent { get; set; }
        public double DepthPercent { get; set; }
        public string Description { get; set; }
        public int GIAReportNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public int Shape { get; set; }
        public double CaratWeight { get; set; }
        public int ColorGrade { get; set; }
        public int ClarityGrade { get; set; }
        public int CutGrade { get; set; }
        public int PolishGrade { get; set; }
        public int SymmetryGrade { get; set; }
        public int FluoresceneGrade { get; set; }
        public string Inscription { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Price { get; set; }
        public bool Block { get; set; }
        public string SKU { get; set; }
        public string? ImageURL { get; set; }
    }
}
