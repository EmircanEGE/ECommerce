using AutoMapper;
using ECommerce.DTOs;
using ECommerce.Entities;

namespace ECommerce.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<CreateOrderDto, OrderDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "Unknown"));
            CreateMap<CreateOrderItemDto, OrderItemDto>().ReverseMap();
        }
    }
}
