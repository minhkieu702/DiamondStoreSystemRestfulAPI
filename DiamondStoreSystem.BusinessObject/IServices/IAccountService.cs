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
    public interface IAccountService
    {
        Task<IDSSResult> GetById(string id);
        Task<IDSSResult> GetAll();
        IDSSResult GetAllWithAllField();
        Task<IDSSResult> GetByEmail(string email);
        Task<IDSSResult> IsExist(string id);
        Task<IDSSResult> Update(string id, AccountRequestModel model);
        Task<IDSSResult> Block(string id);
        Task<IDSSResult> Create(AccountRequestModel model);
        IDSSResult GetEnum();
        Task<IDSSResult> Delete(string id);
        Task<IDSSResult> UnBlock(string id);
        Task<IDSSResult> CheckLogin(string email, string password);
        Task<IDSSResult> ChangeWorkingSchedule(string id, int schedule);
    }
}
