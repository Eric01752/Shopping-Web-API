using AutoMapper;
using ShoppingApp.Models;
using ShoppingApp.Models.Dto;

namespace ShoppingApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandDto, Brand>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItemsInboundDto, OrderItems>();
        }
    }
}
