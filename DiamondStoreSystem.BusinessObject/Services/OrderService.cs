using AutoMapper;
using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.Helper;
using DiamondStoreSystem.BusinessLayer.Helpers;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;
using DiamondStoreSystem.Repositories.Repositories;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Security;
using System.Text;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVnPaymentRepository _vnPaymentRepository;
        private readonly IWarrantyRepository _warrantyRepository;
        private readonly ISubDiamondRepository _subDiamondRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _config;
        private readonly ISubDiamondService _subDiamondService;
        private readonly IWarrantyService _warrantyService;
        private readonly IAccessoryService _accessoryService;
        private readonly IDiamondService _diamondService;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IOrderRepository orderRepository, IAccessoryService accessoryService, IDiamondService diamondService, IWarrantyService warrantyService, IProductService productService, IAccountService accountService, ISubDiamondService subDiamondService, IConfiguration configuration, IVnPaymentRepository vnPaymentRepository, IProductRepository productRepository, ISubDiamondRepository subDiamondRepository, IWarrantyRepository warrantyRepository)
        {
            _warrantyRepository = warrantyRepository;
            _subDiamondRepository = subDiamondRepository;
            _productRepository = productRepository;
            _config = configuration;
            _subDiamondService = subDiamondService;
            _warrantyService = warrantyService;
            _accessoryService = accessoryService;
            _diamondService = diamondService;
            _productService = productService;
            _accountService = accountService;
            _orderRepository = orderRepository;
            _vnPaymentRepository = vnPaymentRepository;
            _mapper = mapper;

        }

        public async Task<IDSSResult> UnBlock(string id)
        {
            try
            {
                var result = await IsExist(id);
                if (result.Status <= 0) return result;

                var order = result.Data as Order;

                var check = await UpdateProperty(order, nameof(order.Block), false);

                if (check.Status <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> Create(OrderRequestModel model)
        {
            try
            {
                var result = await GetById(model.OrderID);

                if (result.Status > 0) return result;

                var order = _mapper.Map<Order>(model);

                //result = await _accountService.IsExist(model.CustomerID);

                //if (result.Status <= 0) return result;

                //order.Customer = result.Data as Account;

                //result = await _accountService.IsExist(model.EmployeeAssignID);

                //if (result.Status <= 0) return result;

                //order.EmployeeAccount = result.Data as Account;

                //order.Products = products;

                _orderRepository.Insert(order);

                var check = _orderRepository.SaveChanges();

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

                var order = result.Data as Order;

                var check = await UpdateProperty(order, nameof(order.Block), true);

                if (check.Status <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> UpdatePriceOrder(string id, double totalPrice)
        {
            try
            {
                var order = await _orderRepository.GetById(id);
                if (order == null) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                order.TotalPrice = totalPrice;

                var check = await Update(order.OrderID, _mapper.Map<OrderRequestModel>(order));

                if (check.Status <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> UpdateStatus(string id, OrderStatus status)
        {
            try
            {
                var order = _orderRepository.GetById(id).Result;

                if (order == null) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                order.OrderStatus = (int)status;

                _orderRepository.Update(order);

                int check = _orderRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> UpdateProperty(Order order, string propertyName, object value)
        {
            try
            {
                var propertyInfo = order.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    return new DSSResult(Const.FAIL_READ_CODE, $"Property '{propertyName}' not found.");

                propertyInfo.SetValue(order, Convert.ChangeType(value, propertyInfo.PropertyType), null);

                await _orderRepository.UpdateById(order, order.OrderID);
                var check = _orderRepository.SaveChanges();
                if (check <= 0)
                    return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);

                return new DSSResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private async Task<AccountResponseModel> AssignToOrderResponse(string accID)
        {
            var result = await _accountService.GetById(accID);
            var acc = result.Data as AccountResponseModel;
            return acc;
        }

        private async Task<Account> AssignToOrder(string accID)
        {
            var result = await _accountService.IsExist(accID);
            var acc = result.Data as Account;
            return acc;
        }

        public async Task<IDSSResult> GetAll()
        {
            try
            {
                var result = await _orderRepository.GetWhere(a => !a.Block);
                if (result.Count() <= 0)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                var orders = result.Select(_mapper.Map<OrderResponseModel>).ToList();
                foreach (var order in orders)
                {
                    order.Customer = await AssignToOrderResponse(order.CustomerID);
                    order.Employee = await AssignToOrderResponse(order.EmployeeAssignID);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orders);
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
                var result = _orderRepository.GetAll();
                if (result == null)
                {
                    return new DSSResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                var orders = result.ToList();
                orders.ForEach(async r =>
                {
                    r.Customer = await AssignToOrder(r.CustomerID);
                    r.EmployeeAccount = await AssignToOrder(r.EmployeeAssignID);
                });
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orders);
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
                var result = await GetAll();
                if (result.Status <= 0)
                {
                    return result;
                }
                var r = (result.Data as List<OrderResponseModel>).FirstOrDefault(o => o.OrderID.Equals(id));
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
                var result = GetAllWithAllField();
                if (result.Status <= 0)
                {
                    return result;
                }
                var r = (result.Data as List<Order>).FirstOrDefault(o => o.OrderID.Equals(id));
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, r);
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
                SupportingFeature.Instance.GetEnumName<PayMethod>(),
                SupportingFeature.Instance.GetEnumName<OrderStatus>(),
            },
            Message = Const.SUCCESS_READ_MSG,
            Status = Const.SUCCESS_READ_CODE,
        };

        public async Task<IDSSResult> Update(string id, OrderRequestModel model)
        {
            try
            {
                if(!id.Equals(model.OrderID)) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                var order = await _orderRepository.GetById(id);
                if (order == null) return new DSSResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                SupportingFeature.Instance.CopyValues(order, _mapper.Map<Order>(model));
                _orderRepository.Update(order);
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

                _orderRepository.Delete(result.Data as Order);

                var check = _orderRepository.SaveChanges();

                if (check <= 0) return new DSSResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

                return new DSSResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<string> GetCart(CartRequestModel model, HttpContext context)
        {
            var result = await CreateCart(model, context);
            if (result.Status <= 0)
            {
                return "Payment failed " + result.Message;
            }
            return result.Message;
        }

        private bool InitOrder(string orderID, string employeeID, string customerID)
        {
            try
            {
                _orderRepository.Insert(new Order
                {
                    OrderID = orderID,
                    EmployeeAssignID = "S002",
                    DateOrdered = DateTime.Now,
                    DateReceived = DateTime.Now,
                    CustomerID = customerID,
                    OrderStatus = (int)OrderStatus.Pending,
                    PayMethod = (int)PayMethod.Online,
                    TotalPrice = 0
                });

                int check = _orderRepository.SaveChanges();
                if (check == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        private bool WorkWithAccessory(string accessoryID, ref double productPrice)
        {
            try
            {
                var result = _accessoryService.UpdateQuantity(accessoryID, "-", 1).Result;
                if (result.Status <= 0) return false;
                var accessory = result.Data as Accessory;
                productPrice += accessory.Price;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool WorkWithDiamond(string diamondID, ref double productPrice)
        {
            try
            {
                var result = _diamondService.Block(diamondID).Result;
                if (result.Status <= 0) return false;
                var diamond = result.Data as Diamond;
                productPrice += diamond.Price;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> InitProduct(string productID, string? accessoryID, string mainDiamondID, string orderID)
        {
            try
            {
                await _productRepository.InsertAsync(new Product
                {
                    ProductID = productID,
                    AccessoryID = string.IsNullOrEmpty(accessoryID) ? null : accessoryID,
                    MainDiamondID = mainDiamondID,
                    Price = 0,
                    OrderID = orderID
                });
                int check = await _productRepository.SaveChangesAsync();
                if (check == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool WorkWithSubDiamond(string subDiamondID, string productID ,ref double productPrice)
        {
            try
            {
                var result = _subDiamondService.Create(new SubDiamondRequestModel
                {
                    ProductID = productID,
                    SubDiamondID = subDiamondID
                }).Result;
                
                if (result.Status <= 0) return false;

                var subdiamnd = result.Data as Diamond;
                
                productPrice += subdiamnd.Price;
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool UpdateProduct(string productID, double productPrice)
        {
            try
            {
                var product = _productRepository.GetById(productID).Result;
                
                if(product == null) return false;
                                
                product.Price = productPrice;
                
                _productRepository.Update(product);

                int check = _productRepository.SaveChanges();
                if (check == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool InitWarranty(string productID)
        {
            try
            {
                var check = _warrantyService.Create(_mapper.Map<WarrantyRequestModel>(new Warranty
                {
                    ExpiredDate = DateTime.Now.AddYears(2),
                    IssueDate = DateTime.Now,
                    WarrantyID = "W" + productID.Substring(1),
                    ProductID = productID
                })).Result;
                if (check.Status <= 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool UpdateOrder(string orderID, double totalPrice)
        {
            try
            {
                var order = _orderRepository.GetById(orderID).Result;

                if(order == null) return false;

                order.TotalPrice = totalPrice;

                _orderRepository.Update(order);


                int check = _orderRepository.SaveChanges();
                if (check == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string PaymentUrl(HttpContext httpContext, double totalPrice, Account customer, string orderID)
        {
            try
            {
                return CreatePaymentUrl(httpContext, new VnPaymentRequestModel
                {
                    Amount = totalPrice,
                    CreatedDate = DateTime.Now,
                    Description = "Thanh toan",
                    FullName = customer.LastName + " " + customer.FirstName,
                    OrderId = orderID
                });
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<IDSSResult> CreateCart(CartRequestModel model, HttpContext context)
        {
            try
            {
                IDSSResult result = new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                IDSSResult failed = new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                string orderID = "O" + DateTime.Now.Ticks;
                double totalPrice = 0;

                // Retrieve customer ID from session or hardcode
                context.Session.TryGetValue("accId", out byte[] session);
                string customerID = session != null ? JsonConvert.DeserializeObject<AuthRequestModel>(Encoding.UTF8.GetString(session)).AccountID : "C001";

                // Verify customer existence
                result = await _accountService.IsExist(customerID);
                if (result.Status <= 0) return result;

                double productPrice = 0;
                string accessoryID = "A000";

                var customer = result.Data as Account;

                // Create order
                if (!InitOrder(orderID, "S002", customerID))
                {
                    return failed;
                }

                // Process each product in the cart
                foreach (var item in model.Cart)
                {
                    // Update accessory quantity if accessory exists
                    if (!string.IsNullOrEmpty(item.AccessoryID))
                    {
                        accessoryID = item.AccessoryID;
                        if (!WorkWithAccessory(item.AccessoryID, ref productPrice))
                        {
                            return failed;
                        }
                    }

                    // Block main diamond and update price
                    if (!WorkWithDiamond(item.MainDiamondID, ref productPrice))
                    {
                        return failed;
                    }

                    // Create product record
                    string productID = "P" + item.MainDiamondID.Substring(1) + accessoryID.Substring(1);

                    if (!await InitProduct(productID, item.AccessoryID, item.MainDiamondID, orderID))
                    {
                        return failed;
                    }

                    // Create warranty record
                    if (!InitWarranty(productID))
                    {
                        return failed;
                    }

                    // Process sub-diamonds and update price
                    foreach (var subDiamondID in item.SubDiamondID)
                    {
                        if (!string.IsNullOrEmpty(subDiamondID))
                        {
                            if (!WorkWithSubDiamond(subDiamondID, productID, ref productPrice))
                            {
                                return failed;
                            }
                        }
                    }

                    // Update product price if necessary
                    if (item.SubDiamondID.Count > 0)
                    {
                        if (!UpdateProduct(productID, productPrice))
                        {
                            return failed;
                        }
                    }

                    totalPrice += productPrice;
                }

                // Update order total price
                if (!UpdateOrder(orderID, totalPrice))
                {
                    return failed;
                }

                // Create payment URL
                var paymentUrl = PaymentUrl(context, totalPrice, customer, orderID);
                if (string.IsNullOrEmpty(paymentUrl))
                {
                    return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }

                return new DSSResult(Const.SUCCESS_CREATE_CODE, paymentUrl);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            try
            {
                var tick = DateTime.Now.Ticks.ToString();
                var vnpay = new VnPayLibrary();
                vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
                vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
                vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
                vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
                vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
                vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locate"]);
                vnpay.AddRequestData("vnp_OrderInfo", "Pay Order:" + model.OrderId);
                vnpay.AddRequestData("vnp_OrderType", "other"); // default value: other
                vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
                vnpay.AddRequestData("vnp_TxnRef", tick);

                var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
                return paymentUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IDSSResult> PaymentExecute(IQueryCollection collections)
        {
            try
            {
                var vnpay = new VnPayLibrary();
                foreach (var (key, value) in collections)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value.ToString());
                    }
                }

                var vnp_OrderId = vnpay.GetResponseData("vnp_TxnRef");
                var vnp_TransactionId = vnpay.GetResponseData("vnp_TransactionNo");
                var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
                var vnp_responseCode = vnpay.GetResponseData("vnp_ResponseCode");
                var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
                if (!checkSignature)
                {
                    return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }

                var orderId = vnp_OrderInfo.Substring(vnp_OrderInfo.IndexOf(":") + 1);
                var vnpayResponse = new VnPaymentResponse
                {
                    OrderId = orderId,
                    Success = true,
                    PaymentMethod = "VnPay",
                    OrderDescription = vnp_OrderInfo,
                    VnpOrderId = vnp_OrderId,
                    TransactionId = vnp_TransactionId,
                    Token = vnp_SecureHash,
                    VnPayResponseCode = vnp_responseCode,
                };

                await _vnPaymentRepository.InsertAsync(vnpayResponse);
                var check = await _vnPaymentRepository.SaveChangesAsync();
                if (check <= 0)
                {
                    return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                if (vnp_responseCode.Equals("00"))
                {
                    var result = UpdateStatus(orderId, OrderStatus.Paid).Result;
                    if (result.Status <= 0)
                    {
                        return result;
                    }
                }
                return new DSSResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, vnpayResponse);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IDSSResult> FilterSearch(Dictionary<string, object> searchParams)
        {
            try
            {
                var result = await GetAll();
                if (result.Status <= 0) return result;
                var orders = result.Data as List<OrderResponseModel>;
                if (searchParams!=null && searchParams.Count>0)
                {
                    orders = SupportingFeature.Instance.FilterModel(orders, searchParams);
                    if (orders.Count == 0) return new DSSResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                return new DSSResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orders);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
