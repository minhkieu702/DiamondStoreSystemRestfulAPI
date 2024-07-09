using AutoMapper;
using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.Helpers;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;
using DiamondStoreSystem.Repositories.Repositories;
using Microsoft.Identity.Client;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IDiamondService _diamondService;
        private readonly IAccessoryService _accessoryServie;
        private readonly IWarrantyService _warrantyService;
        private readonly ISubDiamondService _subDiamondService;

        public ProductService(IMapper mapper, IProductRepository productRepository, ISubDiamondService subDiamondService, IWarrantyService warrantyService, IAccessoryService accessoryService, IDiamondService diamondService)
        {
            _diamondService = diamondService;
            _accessoryServie = accessoryService;
            _warrantyService = warrantyService;
            _subDiamondService = subDiamondService;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IDSSResult> UnBlock(string id)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                var product = result.Data as Product;

                var check = await UpdateProperty(product, nameof(product.Block), false);

                if (check.Status <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Create(ProductRequestModel model)
        {
            try
            {
                var result = await _productRepository.GetById(model.ProductID);
                if (result != null) return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                _productRepository.Insert(_mapper.Map<Product>(model));
                var check = _productRepository.SaveChanges();
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

                var product = result.Data as Product;

                var check = await UpdateProperty(product, nameof(product.Block), true);

                if (check.Status <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> UpdateProperty(Product product, string propertyName, object value)
        {
            try
            {
                var propertyInfo = product.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    return new DSSResult(Const.FAIL_READ_CODE, $"Property '{propertyName}' not found.");

                propertyInfo.SetValue(product, Convert.ChangeType(value, propertyInfo.PropertyType), null);

                await _productRepository.UpdateById(product, product.ProductID);
                var check = _productRepository.SaveChanges();
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
                var result = await _productRepository.GetWhere(a => !a.Block);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                List<ProductResponseModel> products = result.Select(_mapper.Map<ProductResponseModel>).ToList();
                products.ToList().ForEach(prodct =>
                {
                    prodct.Accessory = _mapper.Map<AccessoryResponseModel>(AssignAccessory(prodct.AccessoryID).Result);

                    prodct.MainDiamond = _mapper.Map<DiamondResponseModel>(AssignDiamond(prodct.MainDiamondID).Result);

                    prodct.Warranty = _mapper.Map<WarrantyResponseModel>(AssignWarranty("W" + prodct.ProductID.Substring(1)).Result);

                    prodct.SubDiamonds = AssignSubDIamond(prodct.ProductID).Select(_mapper.Map<SubDiamondResponseModel>).ToList();
                });
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products.ToList());
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> GetAllWithAllField()
        {
            try
            {
                var products = _productRepository.GetAll();
                if (products == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }

                foreach (var product in products.ToList())
                {
                    product.MainDiamond = AssignDiamond(product.MainDiamondID).Result;

                    product.Accessory = AssignAccessory(product.AccessoryID).Result;

                    product.SubDiamonds = AssignSubDIamond(product.ProductID);

                    product.Warranty = AssignWarranty("W" + product.ProductID.Substring(1)).Result;
                }

                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products.ToList());
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private async Task<Accessory> AssignAccessory(string id)
        {
            try
            {
                var result = await _accessoryServie.IsExist(id);
                return result.Status <= 0 ? null : result.Data as Accessory;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private async Task<Warranty> AssignWarranty(string id)
        {
            try
            {
                var result = await _warrantyService.IsExist(id);
                return result.Status <= 0 ? null : result.Data as Warranty;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private async Task<Diamond> AssignDiamond(string id)
        {
            try
            {
                var result = await _diamondService.IsExist(id);
                return result.Status <= 0 ? null : result.Data as Diamond;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private List<SubDiamond> AssignSubDIamond(string productID)
        {
            try
            {
                var result = _subDiamondService.GetAllWithAllField();
                if (result.Status <= 0) return new List<SubDiamond>();
                var subdiamonds = (result.Data as List<SubDiamond>).ToList();
                return subdiamonds.Where(s => s.ProductID.Equals(productID)).ToList();
            }
            catch (Exception)
            {
                return new List<SubDiamond>();
                throw;
            }
        }

        public async Task<IDSSResult> GetById(string id)
        {
            try
            {
                var result = await GetAll();

                if (result.Status <= 0) return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);

                var product = (result.Data as List<ProductResponseModel>).ToList().FirstOrDefault(p => p.ProductID.Equals(id));

                if (product == null) return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);

                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
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
                var result = await GetAllWithAllField();

                if (result.Status <= 0) return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);

                var product = (result.Data as List<Product>).ToList().FirstOrDefault(p => p.ProductID.Equals(id));

                if (product == null) return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);

                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Update(string id, ProductRequestModel model)
        {
            try
            {
                if (!id.Equals(model.OrderID))
                {
                    return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                var result = await _productRepository.GetById(id);
                
                if (result == null) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                SupportingFeature.Instance.CopyValues(result, _mapper.Map<Product>(model));

                _productRepository.Update(result);

                var check = _productRepository.SaveChanges();

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
                var product = await _productRepository.GetById(id);
                IDSSResult result = new DSSResult();
                if (product == null) return new DSSResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                var context = _productRepository.GetDBContext();

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        {
                            #region Delete SubDiamond
                            result = _subDiamondService.GetAllWithAllField();
                            var subdiamonds = result.Data as List<SubDiamond>;
                            foreach (var subdiamond in subdiamonds)
                            {
                                result = await _diamondService.UnBlock(subdiamond.SubDiamondID);
                                if (result.Status <= 0)
                                {
                                    await transaction.RollbackAsync();
                                    return result;
                                }

                                result = await _subDiamondService.Delete(subdiamond.ProductID, nameof(subdiamond.ProductID));
                                if (result.Status <= 0)
                                {
                                    await transaction.RollbackAsync();
                                    return result;
                                }
                            }
                            #endregion

                            #region Unblock Diamond
                            if (product.MainDiamondID != null)
                            {
                                result = await _diamondService.UnBlock(product.MainDiamondID);
                                if (result.Status <= 0)
                                {
                                    await transaction.RollbackAsync();
                                    return result;
                                }
                            }
                            #endregion

                            #region Restock Accessory
                            if (product.AccessoryID != null)
                            {
                                result = await _accessoryServie.UpdateQuantity(product.AccessoryID, "+", 1);
                                if (result.Status <= 0)
                                {
                                    await transaction.RollbackAsync();
                                    return result;
                                }
                            }
                            #endregion

                            #region Delete Warranty
                            result = await _warrantyService.Delete(product.ProductID, nameof(product.Warranty.ProductID));
                            if (result.Status <= 0)
                            {
                                await transaction.RollbackAsync();
                                return result;
                            }
                            #endregion

                            _productRepository.Delete(product);

                            var check = _productRepository.SaveChanges();

                            if (check <= 0)
                            {
                                await transaction.RollbackAsync();
                                return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                            }
                            await transaction.CommitAsync();
                            return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> FilterList(Dictionary<string, object> filters)
        {
            try
            {
                var all = _productRepository.GetAll();
                var products = all.Where(d => !d.Block).ToList();
                if (filters != null && filters.Count > 0)
                {
                    products = SupportingFeature.Instance.FilterModel(products, filters);
                    if (products.Count() <= 0)
                    {
                        return new DSSResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                    }
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products.Select(_mapper.Map<ProductResponseModel>));
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
