using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DataTransferObjects;
using ProductShop.Models;
using ProductShop.ViewModels;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        static string DEFAULT_DIRECTORY_PATH = "../../../Datasets";
        static string path;

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //ResetDatabase(context);



            //path = File.ReadAllText($"{DEFAULT_DIRECTORY_PATH}/users.json");
            //Console.WriteLine(ImportUsers(context, path));


            //path = File.ReadAllText($"{DEFAULT_DIRECTORY_PATH}/products.json");

            //Console.WriteLine(ImportProducts(context, path));


            //path = File.ReadAllText($"{DEFAULT_DIRECTORY_PATH}/categories.json");

            //Console.WriteLine(ImportCategories(context, path));


            //path = File.ReadAllText($"{DEFAULT_DIRECTORY_PATH}/categories-products.json");

            //Console.WriteLine(ImportCategoryProducts(context, path));

            //// Directory.CreateDirectory($"{DEFAULT_DIRECTORY_PATH}/Results");


            //var productsInRange=GetProductsInRange(context);

            ////File.WriteAllText($"{ DEFAULT_DIRECTORY_PATH}/Results/products-in-range.json", productsInRange);

            //Console.WriteLine(productsInRange);

            //var soldProducts = GetSoldProducts(context);


            //Console.WriteLine(soldProducts);

            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //var userWithProductsOutputModel = GetUsersWithProducts(context);

            //File.WriteAllText($"{ DEFAULT_DIRECTORY_PATH}/Results/users-and-products.json", userWithProductsOutputModel);

            //Console.WriteLine(userWithProductsOutputModel);

            







            
            ;

        }


        //--1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var users = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);


            var usersToImport = mapper.Map<IEnumerable<User>>(users);

            context.Users.AddRange(usersToImport);

            


            context.SaveChanges();



            return $"Successfully imported {usersToImport.Count()}";
        }

        //--2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var products = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            var productsToImport = mapper.Map<IEnumerable<Product>>(products);

            context.Products.AddRange(productsToImport);

            context.SaveChanges();
 
            return $"Successfully imported {productsToImport.Count()}";




        }

        //--3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var categories = JsonConvert.DeserializeObject<IEnumerable<CategoriesInputModel>>(inputJson)
                .Where(x=>x.Name!=null).ToList();

            var categoriesToImport = mapper.Map<IEnumerable<Category>>(categories);

            context.Categories.AddRange(categoriesToImport);

            context.SaveChanges();


            return $"Successfully imported {categoriesToImport.Count()}"; ;

        }

        //-4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var categoryProducts = JsonConvert.DeserializeObject <IEnumerable<CategoryProductInputModel>>(inputJson);

            var categoryProductsToImport = mapper.Map<IEnumerable<CategoryProduct>>(categoryProducts);

            context.CategoryProducts.AddRange(categoryProductsToImport);

            context.SaveChanges();

            

            return $"Successfully imported {categoryProductsToImport.Count()}";


        }

        //--5
        public static string GetProductsInRange(ProductShopContext context)
        {
            InitializeMapper();


            var products=context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .ProjectTo<ProductInRangeViewModel>(mapper.ConfigurationProvider)
                .OrderBy(x => x.Price)
                .ToList();


            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            var output = JsonConvert.SerializeObject(products,Formatting.Indented,serializerSettings);

            return output;

        }

        //-6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x =>x.ProductsSold.Any(ps => ps.BuyerId != null))
                .Select(x=> new
                {
                    firstName=x.FirstName,
                    lastName=x.LastName,
                    soldProducts = x.ProductsSold.Select(ps=> new
                    {
                        name=ps.Name,
                        price=ps.Price,
                        buyerFirstName=ps.Buyer.FirstName,
                        buyerLastName=ps.Buyer.LastName
                    })
                })
                .OrderBy(x=>x.lastName)
                .ThenBy(x=>x.firstName)
                .ToList();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;

        }

        //-7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories.Select(x => new
            {
                category = x.Name,

                productsCount = x.CategoryProducts.Count(),

                averagePrice = x.CategoryProducts.Average(a => a.Product.Price).ToString("F2"),

                totalRevenue = x.CategoryProducts.Sum(s => s.Product.Price).ToString("F2"),

            })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(categories,Formatting.Indented);

            return json;



        }

        //-8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x=>x.ProductsSold)
                .ToList()
                .Where(x => x.ProductsSold.Any(ps => ps.BuyerId != null))              
                .Select(x => new
                {
                    firstName=x.FirstName,
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new 
                    {
                       count = x.ProductsSold.Count(b => b.BuyerId != null),
                       products= x.ProductsSold
                       .Where(b => b.BuyerId != null)
                       .Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price
                        })                   
                    .ToList()
                    },
                   

                })
                .OrderByDescending(x=>x.soldProducts.count)
                .ToList();

            var userOutputDTO = new
            {
                usersCount = users.Count(),

                users = users
            };

            var options = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting=Formatting.Indented

            };

            var json = JsonConvert.SerializeObject(userOutputDTO,options);

            return json;

        }


        public static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new ProductShopProfile());
            });

            mapper = config.CreateMapper();
        }
        private static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}