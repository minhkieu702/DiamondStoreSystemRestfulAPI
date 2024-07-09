using DiamondStoreSystem.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResponseModels
{
    public class CertificateResponseModel
    {
        public int GIAReportNumber { get; set; }
        public string Shape { get; set; }
        public string CutGrade { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double CaratWeight { get; set; }
        public string ColorGrade { get; set; }
        public string ClarityGrade { get; set; }
        public string PolishGrade { get; set; }
        public string SymmetryGrade { get; set; }
        public string FluoresceneGrade { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
