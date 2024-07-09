using DiamondStoreSystem.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.ResquestModels
{
    public class AuthRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public Role? Role{ get; set; }
        [JsonIgnore]
        public string? AccountID { get; set; }
    }
}
