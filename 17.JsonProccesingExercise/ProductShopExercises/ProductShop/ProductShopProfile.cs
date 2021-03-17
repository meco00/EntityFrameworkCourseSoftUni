using AutoMapper;
using ProductShop.DataTransferObjects;
using ProductShop.Models;
using ProductShop.ViewModels;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserInputModel, User>();

            this.CreateMap<ProductInputModel, Product>();

            this.CreateMap<CategoriesInputModel, Category>();

            this.CreateMap<CategoryProductInputModel, CategoryProduct>();

            this.CreateMap<Product, ProductInRangeViewModel>()
                .ForMember(x => x.Seller, p => p.MapFrom(x =>x.Seller.FirstName+ " "+ x.Seller.LastName));
            

            
        }
    }
}
