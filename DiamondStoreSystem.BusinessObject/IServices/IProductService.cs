using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.IServices
{
    public interface IProductService
    {
        Task<IDSSResult> FilterList(Dictionary<string, object> filters);
        Task<IDSSResult> GetById(string id);
        Task<IDSSResult> GetAll();
        Task<IDSSResult> GetAllWithAllField();
        Task<IDSSResult> IsExist(string id);
        Task<IDSSResult> Update(string id, ProductRequestModel model);
        Task<IDSSResult> Block(string id);
        Task<IDSSResult> Create(ProductRequestModel model);
        Task<IDSSResult> UpdateProperty(Product product, string propertyName, object value);
        Task<IDSSResult> Delete(string id);
        Task<IDSSResult> UnBlock(string id);
    }
}
