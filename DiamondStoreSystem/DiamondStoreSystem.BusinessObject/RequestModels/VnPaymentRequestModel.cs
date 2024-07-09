using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResquestModels
{
    public class VnPaymentRequestModel
    {
        public string OrderId { get; set; }
        public string Description { get; set; } = string.Empty;
        //[JsonIgnore]
        public string FullName { get; set; }
        [JsonIgnore]
        public double Amount { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
}
