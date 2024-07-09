using AutoMapper;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.BusinessLayer.Commons;

namespace DiamondStoreSystem.API.AppStart
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            #region Account
            CreateMap<Account, AccountRequestModel>().ReverseMap();
            CreateMap<Account, EmployeeResponseModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ((Gender)src.Gender).ToString()))
                .ForMember(dest => dest.WorkingSchedule, opt => opt.MapFrom(src => ((WorkingSchedule)src.WorkingSchedule).ToString()))
                .ReverseMap();
            CreateMap<Account, CustomerResponseModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ((Gender)src.Gender).ToString()))
                .ReverseMap();
            CreateMap<AccountRequestModel, CustomerResponseModel>().ReverseMap();
            CreateMap<AccountRequestModel, EmployeeResponseModel>().ReverseMap();
            CreateMap<Account, AccountResponseModel>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ((Gender)src.Gender).ToString()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => ((Role)src.Role).ToString()))
                .ForMember(dest => dest.WorkingSchedule, opt => opt.MapFrom(src => ((WorkingSchedule)src.WorkingSchedule).ToString())).ReverseMap();
            #endregion

            #region Accessory
            // Map Accessory to AccessoryResponseModel
            CreateMap<Accessory, AccessoryResponseModel>()
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => ((Material)src.Material).ToString()))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => ((Style)src.Style).ToString()))
                .ReverseMap();

            // Map AccessoryRequestModel to Accessory
            CreateMap<AccessoryRequestModel, Accessory>()
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => (int)src.Material))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => (int)src.Style))
                .ReverseMap();
            #endregion

            #region Diamond
            CreateMap<Diamond, DiamondResponseModel>()
                .ForMember(dest => dest.LabCreated, opt => opt.MapFrom(src => ((LabCreated)src.LabCreated).ToString()))
                .ForMember(dest => dest.Shape, opt => opt.MapFrom(src => ((Shape)src.Shape).ToString()))
                .ForMember(dest => dest.ColorGrade, opt => opt.MapFrom(src => ((ColorGrade)src.ColorGrade).ToString()))
                .ForMember(dest => dest.ClarityGrade, opt => opt.MapFrom(src => ((ClarityGrade)src.ClarityGrade).ToString()))
                .ForMember(dest => dest.CutGrade, opt => opt.MapFrom(src => ((Grade)src.CutGrade).ToString()))
                .ForMember(dest => dest.PolishGrade, opt => opt.MapFrom(src => ((Grade)src.PolishGrade).ToString()))
                .ForMember(dest => dest.SymmetryGrade, opt => opt.MapFrom(src => ((Grade)src.SymmetryGrade).ToString()))
                .ForMember(dest => dest.FluoresceneGrade, opt => opt.MapFrom(src => ((Grade)src.FluoresceneGrade).ToString()))
                .ReverseMap();
            CreateMap<Diamond, DiamondRequestModel>().ReverseMap();
            CreateMap<DiamondRequestModel, DiamondResponseModel>().ReverseMap();
            #endregion

            #region Order
            CreateMap<Order, OrderResponseModel>()
           .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => ((OrderStatus)src.OrderStatus).ToString()))
           .ForMember(dest => dest.PayMethod, opt => opt.MapFrom(src => ((PayMethod)src.PayMethod).ToString()))
           .ReverseMap();
            CreateMap<Order, OrderRequestModel>().ReverseMap();
            CreateMap<OrderRequestModel, OrderResponseModel>().ReverseMap();
            #endregion

            CreateMap<Product, ProductResponseModel>().ReverseMap();
            CreateMap<Product, ProductRequestModel>().ReverseMap();
            CreateMap<ProductRequestModel, ProductResponseModel>().ReverseMap();

            CreateMap<DiamondResponseModel, CertificateResponseModel>()
                .ReverseMap();
            CreateMap<Diamond, CertificateResponseModel>()
                .ForMember(dest => dest.Shape, opt => opt.MapFrom(src => ((Shape)src.Shape).ToString()))
                .ForMember(dest => dest.ColorGrade, opt => opt.MapFrom(src => ((ColorGrade)src.ColorGrade).ToString()))
                .ForMember(dest => dest.ClarityGrade, opt => opt.MapFrom(src => ((ClarityGrade)src.ClarityGrade).ToString()))
                .ForMember(dest => dest.CutGrade, opt => opt.MapFrom(src => ((Grade)src.CutGrade).ToString()))
                .ForMember(dest => dest.PolishGrade, opt => opt.MapFrom(src => ((Grade)src.PolishGrade).ToString()))
                .ForMember(dest => dest.SymmetryGrade, opt => opt.MapFrom(src => ((Grade)src.SymmetryGrade).ToString()))
                .ForMember(dest => dest.FluoresceneGrade, opt => opt.MapFrom(src => ((Grade)src.FluoresceneGrade).ToString()))
                .ReverseMap();

            CreateMap<Warranty, WarrantyResponseModel>().ReverseMap();
            CreateMap<Warranty, WarrantyRequestModel>().ReverseMap();

            CreateMap<SubDiamond, SubDiamondResponseModel>().ReverseMap();
            CreateMap<SubDiamond, SubDiamondRequestModel>().ReverseMap();
        }
    }
}
