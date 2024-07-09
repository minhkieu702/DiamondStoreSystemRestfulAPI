using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.IServices
{
    public interface IAuthService
    {
        Task<IDSSResult> Login(string email, string password);
        Task<IDSSResult> Register(AccountRequestModel accountRequestModel, string confirmingCode, HttpContext context);
        Task<IDSSResult> ConfirmEmail(string email, HttpContext context);
        Task<IDSSResult> SetNewPassword(string password, string confirmingCode, HttpContext context);
    }
}
