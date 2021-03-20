using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserInputViewModel, User>();

            this.CreateMap<ProductInputViewModel, Product>();

            this.CreateMap<CategoriesInputViewModel, Category>();

            this.CreateMap<CategoryProductInputViewModel, CategoryProduct>();

            this.CreateMap<Product, ProductsInRangeViewModel>()
            .ForMember(x => x.BuyerName, p => p.MapFrom(x => x.Buyer.FirstName + " " + x.Buyer.LastName));


        }
    }
}
