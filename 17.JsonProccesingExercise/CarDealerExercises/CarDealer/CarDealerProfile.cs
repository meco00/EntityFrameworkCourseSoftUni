using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.DTO.CarDTOs;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierInputDTO, Supplier>();

            this.CreateMap<PartInputDTO, Part>();

            //this.CreateMap<int, PartCar>()
            //    .ForMember(x => x.PartId, cp => cp.MapFrom(x => x));
               
            //this.CreateMap<CarInputDTO, Car>()
            // .ForMember(x => x.PartCars, ci => ci.MapFrom(x => x.PartsId));

            this.CreateMap<CustomerInputDTO, Customer>();

            this.CreateMap<SaleInputDTO, Sale>();

            this.CreateMap<Customer, GetOrderedCustomersDTO>()
            .ForMember(x => x.BirthDate,
            c => c.MapFrom(x => x.BirthDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture)));

            this.CreateMap<Car, CarsByToyotaDTO>();

            this.CreateMap<Supplier, LocalSuppliersViewDTO>()
                .ForMember(x => x.PartsCount, s => s.MapFrom(x => x.Parts.Count))
                .ForMember(x=>x.Name , s=>s.MapFrom(x=>x.Name));


            this.CreateMap<PartCar, PartsViewDTO>()
                .ForMember(x => x.Name, pc => pc.MapFrom(x => x.Part.Name))
                .ForMember(x => x.Price, pc => pc.MapFrom(x => x.Part.Price.ToString("F2")));


            this.CreateMap<Car, CarWithListOfPartsViewDTO>()
                .ForMember(x => x.Parts, c => c.MapFrom(x => x.PartCars));
               
              
                

           





           






        }
    }
}
