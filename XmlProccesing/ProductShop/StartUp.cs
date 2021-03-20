using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        static string path;

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            // path = File.ReadAllText("../../../Datasets/users.xml");

            //Console.WriteLine(ImportUsers(context, path));

            // path= File.ReadAllText("../../../Datasets/products.xml");

            //Console.WriteLine(ImportProducts(context,path));

            //path = File.ReadAllText("../../../Datasets/categories.xml");

            //Console.WriteLine(ImportCategories(context,path));

            //path = File.ReadAllText("../../../Datasets/categories-products.xml");

            //Console.WriteLine(ImportCategoryProducts(context,path));

            //Console.WriteLine( GetProductsInRange(context));


            Console.WriteLine(GetUsersWithProducts(context));

            var arr = Console.ReadLine().Split().ToArray();
          

        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
             cfg.AddProfile(new ProductShopProfile())
             );

            mapper = config.CreateMapper();
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(UserInputViewModel[]),new XmlRootAttribute("Users"));

            var textReader = new StringReader(inputXml);

            var userInputView = (UserInputViewModel[])serializer.Deserialize(textReader);

            var usersToImport = mapper.Map<User[]>(userInputView);

            context.Users.AddRange(usersToImport);

            context.SaveChanges();

            return $"Successfully imported {usersToImport.Length}";

        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(ProductInputViewModel[]), new XmlRootAttribute("Products"));

            var textReader = new StringReader(inputXml);

            var productsInputModels = (ProductInputViewModel[])serializer.Deserialize(textReader);

            var productsToImport = mapper.Map<Product[]>(productsInputModels);

            ;

            context.Products.AddRange(productsToImport);

            context.SaveChanges();

            return $"Successfully imported {productsToImport.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(CategoriesInputViewModel[]), new XmlRootAttribute("Categories"));

            var textReader = new StringReader(inputXml);

            var categoriesInputModels = (CategoriesInputViewModel[])serializer.Deserialize(textReader);

            var categoriesToImport = categoriesInputModels
                .Where(x => x.Name != null)
                .AsQueryable()
                .ProjectTo<Category>(mapper.ConfigurationProvider)
                .ToArray();

            ;

            context.Categories.AddRange(categoriesToImport);

            context.SaveChanges();

            return $"Successfully imported {categoriesToImport.Length}";




        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(CategoryProductInputViewModel[]), new XmlRootAttribute("CategoryProducts"));

            var textReader = new StringReader(inputXml);

            var categoriesProductsInputModels = (CategoryProductInputViewModel[])serializer.Deserialize(textReader);

            var categoriesId = context.Categories.Select(x => x.Id).ToList();

            var productsId = context.Products.Select(x => x.Id).ToList();

            var categoryProductsToImport = categoriesProductsInputModels
                .Where(x => categoriesId.Contains(x.CategoryId) && productsId.Contains(x.ProductId))
                .AsQueryable()
                .ProjectTo<CategoryProduct>(mapper.ConfigurationProvider)
                .ToList();

            ;

            context.CategoryProducts.AddRange(categoryProductsToImport);

            context.SaveChanges();

            return $"Successfully imported {categoryProductsToImport.Count}";



        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(List<ProductsInRangeViewModel>), new XmlRootAttribute("Products"));

            var productsInRange = context.Products
                .Where(x=>x.Price>=500&&x.Price<=1000)
                .OrderBy(x => x.Price)
                .Take(10)
                .ProjectTo<ProductsInRangeViewModel>(mapper.ConfigurationProvider)
                .ToList();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var textWriter = new StringWriter();

             serializer.Serialize(textWriter, productsInRange,ns);

            

            return textWriter.ToString();





        }

        //public static string GetSoldProducts(ProductShopContext context)
        //{
        //    var serializer = 
        //        new XmlSerializer(typeof(SoldUsersProductOutputModel[]), new XmlRootAttribute("Users"));

        //    //var soldUsersProducts = context.Users.Where(x => x.ProductsSold.Count >= 1)
        //    //    .Select(x => new SoldUsersProductOutputModel
        //    //    {
        //    //        FirstName = x.FirstName,
        //    //        LastName = x.LastName,
        //    //        SoldProducts = x.ProductsSold.Select(p => new ProductOutputModel
        //    //        {
        //    //            Name = p.Name,
        //    //            Price = p.Price
        //    //        })
        //    //        .ToArray()
        //    //    })
        //        .OrderBy(x=>x.LastName)
        //        .ThenBy(x=>x.FirstName)
        //        .Take(5)
        //        .ToArray();

            

        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("", "");

        //    var stringWriter = new StringWriter();

        //    serializer.Serialize(stringWriter, soldUsersProducts,ns);


        //    return stringWriter.ToString();
                




        //}

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var serializer =
                new XmlSerializer(typeof(CategoriesByProductCountOutputModel[]), new XmlRootAttribute("Categories"));


            var categoriesProducts = context.Categories.Select(x => new CategoriesByProductCountOutputModel
            {
                Name = x.Name,
                Count = x.CategoryProducts.Count(),
                AveragePrice = x.CategoryProducts.Average(p => p.Product.Price),
                TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)
            })
                .OrderByDescending(x=>x.Count)
                .ThenBy(x=>x.TotalRevenue)
                .ToArray();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");


            var stringWriter = new StringWriter();

            serializer.Serialize(stringWriter, categoriesProducts, ns);

            return stringWriter.ToString();


        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var serializer =
               new XmlSerializer(typeof(UsersWithProductsOuputModel), new XmlRootAttribute("Users"));

            var users = context
                .Users
                .Include(x => x.ProductsSold)
                .ToList()
                .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                .OrderByDescending(x => x.ProductsSold.Count)
                .Select(x => new SoldUsersProductOutputModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new ProductWithCountOutputModel
                    {
                        Count = x.ProductsSold
                        .Count(b => b.Buyer != null),

                        ProductOutputModels = x.ProductsSold

                        .Where(b => b.Buyer != null)
                        .Select(p => new ProductOutputModel
                        {
                            Price = p.Price,
                            Name = p.Name
                        })
                         .OrderByDescending(p => p.Price)
                         .ToArray()

                    },

                })
                .Take(10)
                .ToList();







            var userModelToExport = new UsersWithProductsOuputModel
            {
                Count = context.Users
                .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                .Count(),

                SoldUsersProductOutputModel = users.ToArray()
            };

            var sb = new StringBuilder();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            ns.Add("", "");

            

            serializer.Serialize(new StringWriter(sb), userModelToExport,ns);

            return sb.ToString();


        }
    }
}

                  