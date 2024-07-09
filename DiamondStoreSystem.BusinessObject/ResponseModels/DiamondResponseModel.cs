using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResponseModels
{
    public class DiamondResponseModel
    { public string DiamondID { get; set; }
        public string Origin { get; set; }
        public string LabCreated { get; set; }
        public double TablePercent { get; set; }
        public double DepthPercent { get; set; }
        public string Description { get; set; }
        public int GIAReportNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string Shape { get; set; }
        public double CaratWeight { get; set; }
        public string ColorGrade { get; set; }
        public string ClarityGrade { get; set; }
        public string CutGrade { get; set; }
        public string PolishGrade { get; set; }
        public string SymmetryGrade { get; set; }
        public string FluoresceneGrade { get; set; }
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
