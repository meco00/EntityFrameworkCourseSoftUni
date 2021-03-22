using AutoMapper;
using CarDealer.DataTransferObjects.Export;
using CarDealer.DataTransferObjects.Import;
using CarDealer.Models;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierInputModel, Supplier>();

            this.CreateMap<PartsInputModel, Part>();

            this.CreateMap<CarInputModel, Car>()
                .ForMember(x=>x.PartCars,ci=>ci.MapFrom(x=>x.PartCars));

            this.CreateMap<PartIdInputModel, PartCar>()
                .ForMember(x => x.PartId, p => p.MapFrom(x => x.Id));

            this.CreateMap<CustomerInputModel, Customer>();

            this.CreateMap<SaleInputModel, Sale>();

            this.CreateMap<Car, CarOutputModel>();

            this.CreateMap<Car, CarFromBMWModel>();

            this.CreateMap<Supplier, LocalSuppliersOutputModel>()
                .ForMember(x => x.PartsCount, s => s.MapFrom(x => x.Parts.Count));

            this.CreateMap<Car, CarsWithPartsOutputModel>()
                .ForMember(x => x.Parts,
                c => c.MapFrom(x => x.PartCars.ToList().OrderByDescending(x=>x.Part.Price)));

            this.CreateMap<PartCar, PartsOutpuModel>()
                .ForMember(x => x.Name, pc => pc.MapFrom(x => x.Part.Name))
                .ForMember(x => x.Price, pc => pc.MapFrom(x => x.Part.Price));

            this.CreateMap<Customer, CustomerOutputModel>()
                 .ForMember(x => x.FullName, c => c.MapFrom(x => x.Name))
                 .ForMember(x => x.BoughtCarsCount, c => c.MapFrom(x => x.Sales.Count))
                 .ForMember(x => x.SpentMoney,
                 c => c.MapFrom(
                     x => x.Sales
                     .Sum(x => x.Car.PartCars.Sum(x => x.Part.Price))));



        }
    }
}
