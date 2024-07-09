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
    public interface IOrderService
    {
        Task<IDSSResult> FilterSearch(Dictionary<string, object> searchParams);
        Task<IDSSResult> GetById(string id);
        Task<IDSSResult> GetAll();
        IDSSResult GetAllWithAllField();
        Task<IDSSResult> IsExist(string id);
        Task<IDSSResult> Update(string id, OrderRequestModel model);
        Task<IDSSResult> Block(string id);
        Task<IDSSResult> Create(OrderRequestModel model);
        IDSSResult GetEnum();
        Task<IDSSResult> Delete(string id);
        Task<string> GetCart(CartRequestModel model, HttpContext context);
        Task<IDSSResult> UpdateStatus(string id, OrderStatus status);
        Task<IDSSResult> UnBlock(string id);
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        Task<IDSSResult> PaymentExecute(IQueryCollection collections);
    }
}
