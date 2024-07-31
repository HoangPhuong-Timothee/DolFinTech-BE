using API.DTOs.Comment;
using API.DTOs.Stock;
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