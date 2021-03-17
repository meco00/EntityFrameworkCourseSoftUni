using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.DTO.CarDTOs;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        const string DEFAULT_PATH = "../../../Datasets/";
        static string path;


        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            // ResetDatabase(context);



            // path = File.ReadAllText($"{DEFAULT_PATH}suppliers.json");

            //Console.WriteLine(ImportSuppliers(context, path));


            //path= File.ReadAllText($"{DEFAULT_PATH}parts.json");

            //Console.WriteLine( ImportParts(context, path));


            //path = File.ReadAllText($"{DEFAULT_PATH}cars.json");

            //Console.WriteLine(ImportCars(context, path));

            //path = File.ReadAllText($"{DEFAULT_PATH}/customers.json");

            //Console.WriteLine(ImportCustomers(context,path));


            //path = File.ReadAllText($"{DEFAULT_PATH}/sales.json");

            //Console.WriteLine(ImportSales(context,path));

            //Console.WriteLine(GetOrderedCustomers(context));

            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //Console.WriteLine(GetLocalSuppliers(context));

            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            Console.WriteLine(GetSalesWithAppliedDiscount(context));


        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesWithDiscount = context.Sales
                .Take(10)
                .Select(x => new
            {
                car = new
                {
                    Make = x.Car.Make,
                    Model = x.Car.Model,
                    TravelledDistance = x.Car.TravelledDistance

                },
                
                customerName = x.Customer.Name,
                Discount = x.Discount.ToString("F2"),
                price = x.Car.PartCars.Sum(p => p.Part.Price).ToString("F2"),
                priceWithDiscount=
                ((x.Car.PartCars.Sum(p => p.Part.Price)) - (x.Car.PartCars.Sum(p => p.Part.Price ) * x.Discount)/100).ToString("F2")



            })
                
                .ToList();


            var json = JsonConvert.SerializeObject(salesWithDiscount,Formatting.Indented);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var totalSalesOfCustomers = context.Customers
               .Where(x => x.Sales.Count > 0)
               .Select(x => new
               {
              
               fullName = x.Name,
                   boughtCars = x.Sales.Count,

                   spentMoney = x.Sales
                       .Sum(c => c.Car.PartCars
                       .Sum(pc => pc.Part.Price))




               })
               .OrderByDescending(x => x.spentMoney)
               .ThenByDescending(x => x.boughtCars)
               .ToList();


            var json = JsonConvert.SerializeObject(totalSalesOfCustomers, Formatting.Indented);

            return json;




        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            InitializeMapper();

            var cars = context.Cars.ProjectTo<CarWithListOfPartsViewDTO>(mapper.ConfigurationProvider).ToList();




            var carsMaster2 = new List<CarMasterOutputDTO>();

            ;

            foreach (var item in cars)
            {
                var currentCarMaster = new CarMasterOutputDTO()
                {
                    Car = item,
                    Parts = item.Parts
                };


                carsMaster2.Add(currentCarMaster);

            }

            var json = JsonConvert.SerializeObject(carsMaster2, Formatting.Indented);



            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            InitializeMapper();

            var localSuppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .ProjectTo<LocalSuppliersViewDTO>(mapper.ConfigurationProvider)
                .ToList();

            var json = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);

            return json;


        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            InitializeMapper();

            var toyotaCars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ProjectTo<CarsByToyotaDTO>(mapper.ConfigurationProvider)
                .ToList();


            var json = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);

            return json;


        }


        public static string GetOrderedCustomers(CarDealerContext context)
        {
            InitializeMapper();

            var getOrderedCustomersDTO = context.Customers

                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ProjectTo<GetOrderedCustomersDTO>(mapper.ConfigurationProvider)
                .ToList();



            var json = JsonConvert.SerializeObject(getOrderedCustomersDTO, Formatting.Indented);

            ;

            return json;


        }


        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var salesInputDTO = JsonConvert.DeserializeObject<IEnumerable<SaleInputDTO>>(inputJson);

            var salesToImport = mapper.Map<IEnumerable<Sale>>(salesInputDTO);

            ;

            context.Sales.AddRange(salesToImport);

            context.SaveChanges();

            return $"Successfully imported {salesToImport.Count()}.";



        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var customersDTO = JsonConvert.DeserializeObject<IEnumerable<CustomerInputDTO>>(inputJson);

            var customerToImport = mapper.Map<IEnumerable<Customer>>(customersDTO);



            context.Customers.AddRange(customerToImport);

            context.SaveChanges();

            return $"Successfully imported {customerToImport.Count()}.";






        }


        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var carsDTO = JsonConvert.DeserializeObject<IEnumerable<CarInputDTO>>(inputJson);

            var cars = new List<Car>();

            foreach (var car in carsDTO)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car.PartsId.Distinct())
                {
                    var currentPart = new PartCar()
                    {
                        Car = currentCar,
                        PartId = partId
                    };

                    currentCar.PartCars.Add(currentPart);

                }

                cars.Add(currentCar);

            }


            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported { cars.Count()}.";


        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var partsDTO = JsonConvert.DeserializeObject<PartInputDTO[]>(inputJson);

            var suppliersId = context.Suppliers.Select(x => x.Id).ToArray();

            var parts = partsDTO
                .Where(x => suppliersId.Contains(x.SupplierId))
                .AsQueryable().ProjectTo<Part>(mapper.ConfigurationProvider)
                .ToArray();

            // var parts2 = mapper.Map<List<Part>>(partsDTO).Where(x => suppliersId.Contains(x.SupplierId)).ToArray();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported { parts.Length}.";




        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();


            var supplierDTO = JsonConvert.DeserializeObject<IEnumerable<SupplierInputDTO>>(inputJson);

            var suppliersToImport = mapper.Map<IEnumerable<Supplier>>(supplierDTO);

            context.Suppliers.AddRange(suppliersToImport);

            context.SaveChanges();

            return $"Successfully imported {suppliersToImport.Count()}.";

        }



        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }

        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}