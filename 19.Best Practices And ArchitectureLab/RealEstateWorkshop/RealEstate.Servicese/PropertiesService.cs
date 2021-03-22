using RealEstates.Servicese.Models;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstates.Datase;
using RealEstates.Modelse;

namespace RealEstates.Servicese
{
   public class PropertiesService : IPropertiesService
    {
        private readonly RealEstateDBContext dbContext;

        public PropertiesService(RealEstateDBContext contextInput)
        {
            dbContext = contextInput;
        }

        public void Add(string district, int price, int floor, int maxFloor, int size, int yardSize, int year, string propertyType, string buildingType)
        {
            var property = new PropertyAd
            {
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor > 255 ? null : (byte)floor,
                TotalFloors = maxFloor <= 0 || maxFloor > 255 ? null : (byte)maxFloor,
                YardSize = size <= 0 ? null : size,
                Year = year <= 1800 ? null : year,
            };

            var dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }
            property.District = dbDistrict;

            var dbPropertyType = dbContext.Types.FirstOrDefault(x => x.Name == propertyType);
            if (dbPropertyType == null)
            {
                dbPropertyType = new Type { Name = propertyType };
            }
            property.Type = dbPropertyType;

            var dbBuildingType = dbContext.BuildingTypes.FirstOrDefault(x => x.Name == buildingType);
            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }
            property.BuildingType = dbBuildingType;

            dbContext.PropertyAds.Add(property);
            dbContext.SaveChanges();
        }

        public decimal AveragePricePerSquareMeter()
        {
            return dbContext.PropertyAds.Where(x => x.Price.HasValue)
                .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public IEnumerable<PropertyInfoDTO> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties =
                dbContext.PropertyAds
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
                .Select(x => new PropertyInfoDTO
                {
                    Size = x.Size,
                    Price = x.Price ?? 0,
                   
                })
                .ToList();
            return properties;
        }
    }
}
