using RealEstates.Datase;
using RealEstates.Services;
using RealEstates.Servicese.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Servicese
{
   public class DistrictsService :IDistrictsService

    {
        private readonly RealEstateDBContext dbContext;

        public DistrictsService(RealEstateDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DistrictInfoDTO> GetMostExpenciveDistricts(int count)
        {
            var districts = dbContext.Districts.Select(x => new DistrictInfoDTO
            {
                Name = x.Name,
                PropertiesCount = x.PropertyAds.Count(),
                AveragePricePerSquareKilometer =
                   x.PropertyAds.Where(p => p.Price.HasValue)
                    .Average(p => p.Price / (decimal)p.Size) ?? 0,
            })
           .OrderByDescending(x => x.AveragePricePerSquareKilometer)
           .Take(count)
           .ToList();

            return districts;
        }

        
    }
}
