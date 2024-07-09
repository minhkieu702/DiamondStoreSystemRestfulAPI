using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.IServices
{
    public interface IAccessoryService
    {
        Task<IDSSResult> GetById(string id);
        Task<IDSSResult> GetAll();
        IDSSResult GetAllWithAllField();
        Task<IDSSResult> IsExist(string id);
        Task<IDSSResult> Update(string id, AccessoryRequestModel model);
        Task<IDSSResult> Block(string id);
        Task<IDSSResult> Create(AccessoryRequestModel model);
        IDSSResult GetEnum();
        Task<IDSSResult> Delete(string id);
        Task<IDSSResult> UpdateQuantity(string id, string change, int quantity);
        Task<IDSSResult> UnBlock(string id);
        IDSSResult GetByCategory(Dictionary<string, object> categories);
    }
}
