using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DataTransferObjects.Export;
using CarDealer.DataTransferObjects.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        static string path;

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            // ResetDatabase(context);

            //path = File.ReadAllText("./Datasets/suppliers.xml");

            //Console.WriteLine(ImportSuppliers(context, path));

            //path = File.ReadAllText("./Datasets/parts.xml");

            //Console.WriteLine(ImportParts(context, path));

            //path = File.ReadAllText("./Datasets/cars.xml");

            //Console.WriteLine(ImportCars(context, path));

            //path = File.ReadAllText("./Datasets/customers.xml");

            //Console.WriteLine(ImportCustomers(context, path));

            //path = File.ReadAllText("./Datasets/sales.xml");

            //Console.WriteLine(ImportSales(context, path));

            //Console.WriteLine(GetCarsWithDistance(context));

            // Console.WriteLine(GetCarsFromMakeBmw(context));

            //  Console.WriteLine(GetLocalSuppliers(context));

           // Console.WriteLine(GetCarsWithTheirListOfParts(context));

            Console.WriteLine(GetTotalSalesByCustomer(context));



        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer =
                new XmlSerializer(typeof(List<SupplierInputModel>), new XmlRootAttribute("Suppliers"));

            var stringReader = new StringReader(inputXml);

            var suppliersInputModel = serializer.Deserialize(stringReader) as List<SupplierInputModel>;

            InitializeMapper();

            var suppliersToImport = mapper.Map<IEnumerable<Supplier>>(suppliersInputModel);

            ;

            context.Suppliers.AddRange(suppliersToImport);

            context.SaveChanges();


            return $"Successfully imported {suppliersInputModel.Count}";

        }


        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer =
               new XmlSerializer(typeof(List<PartsInputModel>), new XmlRootAttribute("Parts"));

            var stringReader = new StringReader(inputXml);

            var partsInputModel = serializer.Deserialize(stringReader) as List<PartsInputModel>;

            InitializeMapper();

            var suppliersId = context.Suppliers.Select(x => x.Id)
                .ToList();

            var partsToImport = partsInputModel
                .Where(x=>suppliersId.Contains(x.SupplierId))
                .AsQueryable()
                .ProjectTo<Part>(mapper.ConfigurationProvider)
                .ToList();

            ;

            context.Parts.AddRange(partsToImport);

            context.SaveChanges();

            return $"Successfully imported {partsToImport.Count}";



        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer =
             new XmlSerializer(typeof(List<CarInputModel>), new XmlRootAttribute("Cars"));

            var stringReader = new StringReader(inputXml);

            var carsInputModel = serializer.Deserialize(stringReader) as List<CarInputModel>;

            var partsIds = context.Parts.Select(x => x.Id).ToList();

            InitializeMapper();

            //var carsToImport2 = carsInputModel
            //    .Select(x => new CarInputModel
            //    {
            //        Make = x.Make,
            //        Model = x.Model,
            //        TravelledDistance = x.TravelledDistance,
            //        PartCars = x.PartCars.Select(x => x.Id).Distinct().Intersect(partsIds)
            //        .Select(x => new PartIdInputModel
            //        {
            //            Id = x
            //        }
            //         )
            //        .ToArray()
            //    })
            //    .AsQueryable()
            //    .ProjectTo<Car>(mapper.ConfigurationProvider)
            //    .ToList();


            var carsToImport = carsInputModel.Select(x => new Car
            {
                Make = x.Make,
                Model = x.Model,
                TravelledDistance = x.TravelledDistance,
                PartCars = x.PartCars.Select(x => x.Id).Distinct().Intersect(partsIds)
                 .Select(x => new PartCar
                 {
                     PartId = x

                 })
                 .ToList()


            })
                .ToList();

            ;

            context.Cars.AddRange(carsToImport);

            context.SaveChanges();
               

            return $"Successfully imported {carsToImport.Count}";

        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer =
            new XmlSerializer(typeof(List<CustomerInputModel>), new XmlRootAttribute("Customers"));

            var stringReader = new StringReader(inputXml);

            var customersInputModel = serializer.Deserialize(stringReader) as List<CustomerInputModel>;

            InitializeMapper();

            var customersToImport = mapper.Map<IEnumerable<Customer>>(customersInputModel);

            ;

            context.Customers.AddRange(customersToImport);

            context.SaveChanges();

            return $"Successfully imported {customersInputModel.Count}";


        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer =
           new XmlSerializer(typeof(List<SaleInputModel>), new XmlRootAttribute("Sales"));

            var stringReader = new StringReader(inputXml);

            var salesInputModel = serializer.Deserialize(stringReader) as List<SaleInputModel>;

            var carsId = context.Cars.Select(x => x.Id).ToList();

            InitializeMapper();

           var salesToImport = salesInputModel
                .Where(x => carsId.Contains(x.CarId))
                .AsQueryable()
                .ProjectTo<Sale>(mapper.ConfigurationProvider)
                .ToList();

            ;

            context.Sales.AddRange(salesToImport);

            context.SaveChanges();

            return $"Successfully imported {salesToImport.Count}";

        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var serializer =
         new XmlSerializer(typeof(List<CarOutputModel>), new XmlRootAttribute("cars"));


            InitializeMapper();

            var carsToOuput = context.Cars
                .Where(x => x.TravelledDistance >= 2_000_000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ProjectTo<CarOutputModel>(mapper.ConfigurationProvider)
                .ToList();

            ;

            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);

            var ns = new  XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(stringWriter, carsToOuput, ns);

            return sb.ToString();

        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var serializer =
        new XmlSerializer(typeof(List<CarFromBMWModel>), new XmlRootAttribute("cars"));

            InitializeMapper();

            var carFromBMW = context.Cars.
                Where(x => x.Make == "BMW")       
                .ProjectTo<CarFromBMWModel>(mapper.ConfigurationProvider)
                 .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            ;

           // var sb = new StringBuilder();

            var stringWriter = new StringWriter();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(stringWriter, carFromBMW, ns);

            return stringWriter.ToString();


        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var serializer =
       new XmlSerializer(typeof(List<LocalSuppliersOutputModel>), new XmlRootAttribute("suppliers"));

            InitializeMapper();

            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .ProjectTo<LocalSuppliersOutputModel>(mapper.ConfigurationProvider)
                .ToList();

            ;

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), suppliers, ns);

            return sb.ToString().Trim();

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var serializer =
      new XmlSerializer(typeof(List<CarsWithPartsOutputModel>), new XmlRootAttribute("cars"));

            InitializeMapper();

            var carsWithParts = context.Cars  
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ProjectTo<CarsWithPartsOutputModel>(mapper.ConfigurationProvider)
                .ToList();

            ;

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), carsWithParts, ns);

            return sb.ToString().Trim();



        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var serializer =
     new XmlSerializer(typeof(List<CustomerOutputModel>), new XmlRootAttribute("customers"));

            InitializeMapper();

            var carsWithParts = context.Customers
                
                .Include(x => x.Sales)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.PartCars)
                .ThenInclude(x => x.Part)
                .ToList()
                .AsQueryable()
                .ProjectTo<CustomerOutputModel>(mapper.ConfigurationProvider)
                .Where(x => x.BoughtCarsCount >= 1)
                .OrderByDescending(x=>x.SpentMoney)
                .ToList();

            ;

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), carsWithParts, ns);

            return sb.ToString().Trim();
        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
              cfg.AddProfile<CarDealerProfile>());

            mapper = config.CreateMapper();
        }

        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}