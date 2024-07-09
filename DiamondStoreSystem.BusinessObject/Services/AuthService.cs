using AutoMapper;
using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.Helper;
using DiamondStoreSystem.BusinessLayer.Helpers;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMailService _mailService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, IAccountService accountService, IMailService mailService)
        {
            _mailService = mailService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<IDSSResult> Login(string email, string password)
        {
            try
            {
                var result = await _accountService.CheckLogin(email, password);

                return result;
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Register(AccountRequestModel accountRequestModel, string confirmingCode, HttpContext context)
        {
            try
            {
                var email = SupportingFeature.Instance.GetValueFromSession("email", context);
                var otp = SupportingFeature.Instance.GetValueFromSession("otp", context);
                if (!otp.ToString().Equals(confirmingCode))
                {
                    return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                accountRequestModel.AccountID = confirmingCode;
                accountRequestModel.Email = email.ToString();
                var result = await _accountService.Create(accountRequestModel);
                return result;
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> ConfirmEmail(string email, HttpContext context)
        {
            try
            {
                var otp = Guid.NewGuid().ToString();
                SupportingFeature.Instance.SetValueToSession("otp", otp, context);
                SupportingFeature.Instance.SetValueToSession("email", email, context);
                var result = await _mailService.SendMail(email, "CONFIRMING CODE", otp);
                return result;
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> SetNewPassword(string password, string confirmingCode, HttpContext context)
        {
            try
            {
                var email = SupportingFeature.Instance.GetValueFromSession("email", context);
                var otp = SupportingFeature.Instance.GetValueFromSession("otp", context);
                if (!otp.ToString().Equals(confirmingCode))
                {
                    return new DSSResult(Const.FAIL_UPDATE_CODE, "Confirming code is not correct. Please enter correctly!");
                }
                var result = await _accountService.GetByEmail(email.ToString());
                if (result.Status <= 0)
                {
                    return new DSSResult(Const.FAIL_UPDATE_CODE, "Email is not existed!!!");
                }
                var account = result.Data as Account;
                account.Password = password;
                result = await _accountService.Update(account.AccountID, _mapper.Map<AccountRequestModel>(account));
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
