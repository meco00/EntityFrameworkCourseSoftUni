using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var db = new ApplicationDbContext();
            //db.Database.Migrate();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Property search");
                Console.WriteLine("2. Most expensive districts");
                Console.WriteLine("3. Average price per square meter");
                Console.WriteLine("4.Add Tag ");
                Console.WriteLine("5.Bulk Add Tags ");
                Console.WriteLine("6. Map Tags To Properties");
                Console.WriteLine("7. Get Full Data About Properties");
                Console.WriteLine("0. EXIT");
                bool parsed = int.TryParse(Console.ReadLine(), out int option);
                if (parsed && option == 0)
                {
                    break;
                }
                
                if (parsed && option >= 1 && option <= 7)
                {
                    switch (option)
                    {
                        case 1:
                            PropertySearch(db);
                            break;
                        case 2:
                            MostExpensiveDistricts(db);
                            break;
                        case 3:
                            AveragePricePerSquareMeter(db);
                            break;
                        case 4:
                            AddTag(db);
                            break;
                        case 5:
                            BulkTagAdding(db);
                            break;
                        case 6:
                            MapTagsToProperties(db);
                            break;
                        case 7:
                            GetFullDataAboutProperties(db);
                            break;

                           
                    }

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void GetFullDataAboutProperties(ApplicationDbContext db)
        {
            IPropertiesService service = new PropertiesService(db);


            Console.Write("Count of properties:");
            bool parse = int.TryParse(Console.ReadLine(), out int count);
            if (!parse)
            {
                return;
            }

            var fullData = service.GetFullData(count).ToList();

            var serializer = 
                new XmlSerializer(typeof(List<PropertyFullInfoDTO>), new XmlRootAttribute("Properties"));

            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();

            ns.Add("", "");

           

            serializer.Serialize(new StringWriter(sb), fullData,ns);


           

            Console.WriteLine(sb.ToString());


            
        }

        private static void AddTag(ApplicationDbContext db)
        {
            Console.Write("TagName:");
            string name = Console.ReadLine();
            Console.Write("Importance (optional) :");
            int result;
            bool tryParse = int.TryParse(Console.ReadLine(),out result);
            ITagService service = new TagService(db, new PropertiesService(db));
            if (tryParse)
            {
              service.Add(name, result);
            }
            else
            {
                service.Add(name);

            }


        }

        private static void MapTagsToProperties(ApplicationDbContext db)
        {
            ITagService tagService = new TagService(db, new PropertiesService(db));
            tagService.MapTagsToProperties();
        }

        private static void BulkTagAdding(ApplicationDbContext context)
        {
            ITagService tagService = new TagService(context,new PropertiesService(context));
            tagService.BulkAdd();
        }

        private static void AveragePricePerSquareMeter(ApplicationDbContext dbContext)
        {
            IPropertiesService propertiesService = new PropertiesService(dbContext);
            Console.WriteLine($"Average price per square meter: {propertiesService.AveragePricePerSquareMeter():0.00}€/m²");
        }

        private static void MostExpensiveDistricts(ApplicationDbContext db)
        {
            Console.Write("Districts count:");
            int count = int.Parse(Console.ReadLine());
            IDistrictsService districtsService = new DistrictsService(db);
            var districts = districtsService.GetMostExpensiveDistricts(count);
            foreach (var district in districts)
            {
                Console.WriteLine($"{district.Name} => {district.AveragePricePerSquareMeter:0.00}€/m² ({district.PropertiesCount})");
            }
        }

        private static void PropertySearch(ApplicationDbContext db)
        {
            Console.Write("Min price:");
            int minPrice = int.Parse(Console.ReadLine());
            Console.Write("Max price:");
            int maxPrice = int.Parse(Console.ReadLine());
            Console.Write("Min size:");
            int minSize = int.Parse(Console.ReadLine());
            Console.Write("Max size:");
            int maxSize = int.Parse(Console.ReadLine());

            IPropertiesService service = new PropertiesService(db);
            var properties = service.Search(minPrice, maxPrice, minSize, maxSize);
            foreach (var property in properties)
            {
                Console.WriteLine($"{property.DistrictName}; {property.BuildingType}; {property.PropertyType} => {property.Price}€ => {property.Size}m²");
            }
        }
    }
}
