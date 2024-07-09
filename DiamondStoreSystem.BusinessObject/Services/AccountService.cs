using AutoMapper;
using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.Helpers;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IMapper mapper, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<IDSSResult> UnBlock(string id)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                var account = result.Data as Account;

                var check = await UpdateProperty(account, nameof(account.Block), false);


                if (check.Status <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Create(AccountRequestModel model)
        {
            try
            {
                var result = await GetByEmail(model.Email);
                if (result.Status > 0) return new DSSResult(Const.FAIL_CREATE_CODE, "This email is already used.");
                model.Password = Util.HashPassword(model.Password);
                _accountRepository.Insert(_mapper.Map<Account>(model));
                var check = _accountRepository.SaveChanges();
                if (check <= 0) return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                return new DSSResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Block(string id)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                var account = result.Data as Account;

                var check = await UpdateProperty(account, nameof(account.Block), true);

                if (check.Status <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> UpdateProperty(Account account, string propertyName, object value)
        {
            try
            {
                var propertyInfo = account.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    return new DSSResult(Const.FAIL_READ_CODE, $"Property '{propertyName}' not found.");

                propertyInfo.SetValue(account, Convert.ChangeType(value, propertyInfo.PropertyType), null);

                await _accountRepository.UpdateById(account, account.AccountID);
                var check = _accountRepository.SaveChanges();
                if (check <= 0)
                    return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> GetAll()
        {
            try
            {
                var result = await _accountRepository.GetWhere(a => !a.Block);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<AccountResponseModel>)
                    .ToList());
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public IDSSResult GetAllWithAllField()
        {
            try
            {
                var result = _accountRepository.GetAll();
                if (result == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> GetById(string id)
        {
            try
            {
                var result = await _accountRepository.GetWhere(a => !a.Block && a.AccountID == id);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<AccountResponseModel>(result.FirstOrDefault(r => true)));
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> IsExist(string id)
        {
            try
            {
                var result = await _accountRepository.GetById(id);
                if (result == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public IDSSResult GetEnum() => new DSSResult()
        {
            Data = new List<Dictionary<int, string>>()
            {
                SupportingFeature.Instance.GetEnumName<Role>(),
                SupportingFeature.Instance.GetEnumName<WorkingSchedule>(),
                SupportingFeature.Instance.GetEnumName<Gender>(),
            },
            Message = Const.SUCCESS_READ_MSG,
            Status = Const.SUCCESS_READ_CODE,
        };

        public async Task<IDSSResult> Update(string id, AccountRequestModel model)
        {
            try
            {
                if (!id.Equals(model.AccountID)) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                var result = await _accountRepository.GetById(id);

                if (result == null) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                SupportingFeature.Instance.CopyValues(result, _mapper.Map<Account>(model));

                result.Password = Util.HashPassword(model.Password);

                _accountRepository.Update(result);

                var check = _accountRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Delete(string id)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                _accountRepository.Delete(result.Data as Account);

                var check = _accountRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> GetByEmail(string email)
        {
            try
            {
                var result = _accountRepository.GetAll().ToList();
                var account = result.FirstOrDefault(r => r.Email.Equals(email));
                if (account == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, account);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> CheckLogin(string email, string password)
        {
            try
            {
                var result = await _accountRepository.GetWhere(x => x.Email == email && x.Password == Util.HashPassword(password) && !x.Block);

                if (result.Count() == 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                var account = result.FirstOrDefault();
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new AuthRequestModel
                {
                    AccountID = account.AccountID,
                    Role = (Role?)account.Role,
                    Email = email
                });
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> ChangeWorkingSchedule(string id, int schedule)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                var account = result.Data as Account;

                account.WorkingSchedule = schedule;

                _accountRepository.Update(account);

                var check = _accountRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
