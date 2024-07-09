using AutoMapper;
using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.Helpers;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;
using DiamondStoreSystem.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class SubDiamondService : ISubDiamondService
    {
        private readonly ISubDiamondRepository _subDiamondRepository;
        private readonly IDiamondService _diamondService;
        private readonly IMapper _mapper;

        public SubDiamondService(IMapper mapper, ISubDiamondRepository subDiamondRepository, IDiamondService diamondService)
        {
            _subDiamondRepository = subDiamondRepository;
            _mapper = mapper;
            _diamondService = diamondService;
        }
        public async Task<IDSSResult> Create(SubDiamondRequestModel model)
        {
            try
            {
                var result = await GetById(model.SubDiamondID);
                if (result.Status > 0) return result;

                _subDiamondRepository.Insert(_mapper.Map<SubDiamond>(model));
                var check = _subDiamondRepository.SaveChanges();
                if (check <= 0) return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);

                result = await _diamondService.Block(model.SubDiamondID);
                if (result.Status <= 0) return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);

                return new DSSResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result.Data);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        //public async Task<IDSSResult> Block(string id)
        //{
        //    try
        //    {
        //        var result = await IsExist(id);
        //        if (result.Status <= 0) return result;

        //        var subDiamond = result.Data as SubDiamond;

        //        var check = await UpdateProperty(subDiamond, nameof(subDiamond.Block), true);

        //        if (check.Status <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

        //        return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
        //    }
        //}

        public async Task<IDSSResult> UpdateProperty(SubDiamond subDiamond, string propertyName, object value)
        {
            try
            {
                var propertyInfo = subDiamond.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    return new DSSResult(Const.FAIL_READ_CODE, $"Property '{propertyName}' not found.");

                propertyInfo.SetValue(subDiamond, Convert.ChangeType(value, propertyInfo.PropertyType), null);

                await _subDiamondRepository.UpdateById(subDiamond, subDiamond.SubDiamondID);
                var check = _subDiamondRepository.SaveChanges();
                if (check <= 0)
                    return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private async Task<Diamond> AssignDiamond(string id)
        {
            var result = await _diamondService.IsExist(id);
            return result.Data as Diamond;
        }

        public async Task<IDSSResult> GetAll()
        {
            try
            {
                var result = _subDiamondRepository.GetAll();
                if (result == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                var r = result.Select(_mapper.Map < SubDiamondResponseModel > ).ToList();
                r.ForEach(async d => d.SubDiamond = _mapper.Map<DiamondResponseModel>(await AssignDiamond(d.SubDiamondID)));
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, r);
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
                var result = _subDiamondRepository.GetAll();
                if (result == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                var r = result.ToList();
                r.ForEach(async d => d.Diamond = await AssignDiamond(d.SubDiamondID));
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, r);
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
                var result = await _subDiamondRepository.GetWhere(a => a.SubDiamondID == id);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                var r = _mapper.Map<SubDiamondResponseModel>(result.FirstOrDefault(r => true));
                r.SubDiamond = _mapper.Map<DiamondResponseModel>(await AssignDiamond(id));
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, r);
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
                var result = await _subDiamondRepository.GetById(id);
                if (result == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                result.Diamond = await AssignDiamond(id);
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
                SupportingFeature.Instance.GetEnumName<Material>(),
                SupportingFeature.Instance.GetEnumName<Style>(),
            },
            Message = Const.SUCCESS_READ_MSG,
            Status = Const.SUCCESS_READ_CODE,
        };

        public async Task<IDSSResult> Update(string id, SubDiamondRequestModel model)
        {
            try
            {
                var result = await GetById(id);
                if (result.Status <= 0) return result;

                await _subDiamondRepository.UpdateById(_mapper.Map<SubDiamond>(model), id);

                var check = _subDiamondRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Delete(string id, string propertyName)
        {
            try
            {
                var result = _subDiamondRepository.GetAll().ToList();

                var subDiamond = result.FirstOrDefault(s => s.GetPropertyValue(propertyName).Equals(id));

                if (subDiamond == null) return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);

                subDiamond.Diamond = null;

                _subDiamondRepository.Delete(subDiamond);

                var check = _subDiamondRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
