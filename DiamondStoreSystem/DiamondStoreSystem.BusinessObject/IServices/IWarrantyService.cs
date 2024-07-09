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
    public interface IWarrantyService
    {
        Task<IDSSResult> GetById(string id);
        Task<IDSSResult> GetAll();
        IDSSResult GetAllWithAllField();
        Task<IDSSResult> IsExist(string id);
        Task<IDSSResult> Update(string id, WarrantyRequestModel model);
        Task<IDSSResult> Block(string id);
        Task<IDSSResult> Create(WarrantyRequestModel model);
        IDSSResult GetEnum();
        Task<IDSSResult> Delete(string id, string propertyName);
        Task<IDSSResult> UnBlock(string id);
        Task<IDSSResult> GetByProp(string id, string prop);
        IDSSResult ExpiredWarranty();
    }
}
