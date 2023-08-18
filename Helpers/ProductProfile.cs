using AutoMapper;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Helpers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductImage, ProductImageDTO>();
            CreateMap<ProductRequest, Product>();
            CreateMap<FavoriteProduct, FavoriteProductDto>();
            CreateMap<FavoriteProductRequest, FavoriteProduct>();
            CreateMap<ReportedProduct, ReportedProductDto>();
            CreateMap<ReportedProductRequest, ReportedProduct>();
        }
    }
}
