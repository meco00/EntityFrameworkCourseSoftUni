using AutoMapper;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services.Profiles
{
    public class RealEstateProfile : Profile
    {
        public RealEstateProfile()
        {
            //Name = x.Name,
            //    PropertiesCount = x.Properties.Count(),
            //    AveragePricePerSquareMeter =
            //        x.Properties.Where(p => p.Price.HasValue)
            //
            //   .Average(p => p.Price / (decimal)p.Size) ?? 0,

            this.CreateMap<District, DistrictInfoDto>()
                .ForMember(x=>x.PropertiesCount,d=>d.MapFrom(x=>x.Properties.Count()))
                .ForMember(x=>x.AveragePricePerSquareMeter,
                d=>
                d.MapFrom(x=> x.Properties
                .Where(p => p.Price.HasValue)
                .Average(p => p.Price / (decimal)p.Size) ?? 0));


            //Size = x.Size,
            //        Price = x.Price ?? 0,
            //        BuildingType = x.BuildingType.Name,
            //        DistrictName = x.District.Name,
            //        PropertyType = x.Type.Name,

            this.CreateMap<Property, PropertyInfoDto>()
                .ForMember(x => x.Price, p => p.MapFrom(x => x.Price ?? 0))
                .ForMember(x => x.BuildingType, p => p.MapFrom(x => x.BuildingType.Name))
                .ForMember(x => x.DistrictName, p => p.MapFrom(x => x.District.Name))
                .ForMember(x => x.PropertyType, p => p.MapFrom(x => x.Type.Name));


            this.CreateMap<Property, PropertyFullInfoDTO>()
                .ForMember(x => x.Price, p => p.MapFrom(x => x.Price ?? 0))
                .ForMember(x => x.BuildingType, p => p.MapFrom(x => x.BuildingType.Name))
                .ForMember(x => x.DistrictName, p => p.MapFrom(x => x.District.Name))
                .ForMember(x => x.PropertyType, p => p.MapFrom(x => x.Type.Name))
                .ForMember(x => x.Tags, p => p.MapFrom(x => x.Tags));


            this.CreateMap<Tag, TagInfoDTO>();






        }

    }
}
