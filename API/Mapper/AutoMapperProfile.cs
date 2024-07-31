using API.DTOs.Comment;
using API.DTOs.Stock;
using API.Helpers;
using API.Models;
using AutoMapper;

namespace API.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Stocks
            CreateMap<Stock, StockDTO>().ReverseMap();
            CreateMap<FMPStock, Stock>()
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.symbol))
            .ForMember(dest => dest.MarketCap, opt => opt.MapFrom(src => src.mktCap))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.companyName))
            .ForMember(dest => dest.Purchase, opt => opt.ConvertUsing(new PurchaseFormatter(), src => src.price))
            .ForMember(dest => dest.LastDiv, opt => opt.MapFrom(src => src.lastDiv))
            .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.industry))
            .ReverseMap();
            CreateMap<CreateStockRequest, Stock>().ReverseMap();
            CreateMap<UpdateStockRequest, Stock>().ReverseMap();

            //Comments
            CreateMap<Comment, CommentDTO>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.AppUser.UserName))
            .ReverseMap();
            CreateMap<CreateCommentRequest, Comment>().ReverseMap();
            CreateMap<UpdateCommentRequest, Comment>().ReverseMap();
        }
    }
}