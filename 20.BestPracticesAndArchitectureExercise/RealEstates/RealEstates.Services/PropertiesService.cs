using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertiesService : BaseClass, IPropertiesService
    {
        private readonly ApplicationDbContext dbContext;

        public PropertiesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(string district, int price,
            int floor, int maxFloor, int size, int yardSize,
            int year, string propertyType, string buildingType)
        {
            var property = new Property
            {
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor > 255 ? null : (byte)floor,
                TotalFloors = maxFloor <= 0 || maxFloor > 255 ? null : (byte)maxFloor,
                YardSize = yardSize <= 0 ? null : yardSize,
                Year = year <= 1800 ? null : year,
            };

            var dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }
            property.District = dbDistrict;

            var dbPropertyType = dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);
            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }
            property.Type = dbPropertyType;

            var dbBuildingType = dbContext.Buildings.FirstOrDefault(x => x.Name == buildingType);
            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }
            property.BuildingType = dbBuildingType;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();
        }

        public decimal AveragePriceOfProperties()
        {
            return dbContext.Properties
                .Where(x=>x.Price.HasValue)
                .Average(x => (decimal)x.Price) ;
                
                
        }

        public decimal AveragePricePerSquareMeter()
        {
            return dbContext.Properties.Where(x => x.Price.HasValue)
                .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public decimal AveragePricePerSquareMeterForDistrict(int districtId)
        {
            return dbContext.Properties
                 .Where(x => x.DistrictId == districtId && x.Price.HasValue)
                 .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public double AverageSize(int districtId)
        {
            return dbContext.Properties
                .Where(x=>x.DistrictId==districtId)
                .Average(x => x.Size);
        }

        public IEnumerable<PropertyFullInfoDTO> GetFullData(int count)
        {
            return this.dbContext.Properties
                .Where(x => x.Year.HasValue && x.Year.Value > 2015)
                .ProjectTo<PropertyFullInfoDTO>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Price)
                .ThenByDescending(x => x.Size)
                .Take(count)
                .ToList();
        }

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties =
                dbContext.Properties
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
                .ProjectTo<PropertyInfoDto>(mapper.ConfigurationProvider)
                .ToList();
            return properties;
        }
    }
}
