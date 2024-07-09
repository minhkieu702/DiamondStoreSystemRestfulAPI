using AutoMapper;
using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.Helpers;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class DiamondService : IDiamondService
    {
        private readonly IDiamondRepository _diamondRepository;
        private readonly IMapper _mapper;

        public DiamondService(IMapper mapper, IDiamondRepository diamondRepository)
        {
            _diamondRepository = diamondRepository;
            _mapper = mapper;
        }

        public async Task<IDSSResult> UnBlock(string id)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                var diamond = result.Data as Diamond;

                var check = await UpdateProperty(diamond, nameof(diamond.Block), false);


                if (check.Status <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Create(DiamondRequestModel model)
        {
            try
            {
                var result = await GetById(model.DiamondID);
                if (result.Status > 0) return result;
                _diamondRepository.Insert(_mapper.Map<Diamond>(model));
                var check = _diamondRepository.SaveChanges();
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

                var diamond = result.Data as Diamond;

                var check = await UpdateProperty(diamond, nameof(diamond.Block), true);

                if (check.Status <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, diamond);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> UpdateProperty(Diamond diamond, string propertyName, object value)
        {
            try
            {
                var propertyInfo = diamond.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    return new DSSResult(Const.FAIL_READ_CODE, $"Property '{propertyName}' not found.");

                propertyInfo.SetValue(diamond, Convert.ChangeType(value, propertyInfo.PropertyType), null);

                await _diamondRepository.UpdateById(diamond, diamond.DiamondID);
                var check = _diamondRepository.SaveChanges();
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
                var result = await _diamondRepository.GetWhere(a => !a.Block);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<DiamondResponseModel>)
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
                var result = _diamondRepository.GetAll();
                if (result.ToList().Count() <= 0)
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
                var result = await _diamondRepository.GetWhere(a => !a.Block && a.DiamondID == id);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<DiamondResponseModel>(result.FirstOrDefault(r => true)));
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
                var result = await _diamondRepository.GetById(id);
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
                SupportingFeature.Instance.GetEnumName<LabCreated>(),
                SupportingFeature.Instance.GetEnumName<Shape>(),
                SupportingFeature.Instance.GetEnumName<ColorGrade>(),
                SupportingFeature.Instance.GetEnumName<ClarityGrade>(),
                SupportingFeature.Instance.GetEnumName<Grade>(),
            },
            Message = Const.SUCCESS_READ_MSG,
            Status = Const.SUCCESS_READ_CODE,
        };


        public async Task<IDSSResult> Update(string id, DiamondRequestModel model)
        {
            try
            {
                if (!id.Equals(model.DiamondID))
                {
                    return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                var result = _diamondRepository.GetById(id).Result;
                
                if (result == null) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                SupportingFeature.Instance.CopyValues(result, _mapper.Map<Diamond>(model));

                _diamondRepository.Update(result);

                var check = _diamondRepository.SaveChanges();

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

                _diamondRepository.Delete(result.Data as Diamond);

                var check = _diamondRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> GetCertificate(string id)
        {
            try
            {
                var result = await GetById(id);
                if (result.Status <= 0)
                {
                    return new DSSResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<CertificateResponseModel>(result.Data as DiamondResponseModel));
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> GetByProp(string keyword, string propertyName)
        {
            try
            {
                var result = await _diamondRepository.GetWhere(a => !a.Block);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                List<DiamondResponseModel> diamonds = new List<DiamondResponseModel>();
                result.ToList().ForEach(d =>
                {
                    if (d.GetPropertyValue(propertyName).ToString().Trim().ToUpper().Contains(keyword.ToString().Trim().ToUpper())) diamonds.Add(_mapper.Map<DiamondResponseModel>(d));
                });
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, diamonds);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public IDSSResult GetByCategory(Dictionary<string, object> categories)
        {
            try
            {
                var allDiamonds = _diamondRepository.GetAll();
                var diamonds = allDiamonds.Where(d => !d.Block).ToList();
                if (categories != null && categories.Count > 0)
                {
                    diamonds = SupportingFeature.Instance.FilterModel(diamonds, categories);
                    if (diamonds.Count <= 0)
                    {
                        return new DSSResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                    }
                }
                if (diamonds.Count() <= 0)
                {
                    return new DSSResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, diamonds.Select(_mapper.Map<DiamondResponseModel>));
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
