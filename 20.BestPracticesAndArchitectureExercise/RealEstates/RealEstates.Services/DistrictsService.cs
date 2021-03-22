using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    public class DistrictsService : BaseClass, IDistrictsService
    {
        private readonly ApplicationDbContext dbContext;

        public DistrictsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = dbContext.Districts
                .ProjectTo<DistrictInfoDto>(mapper.ConfigurationProvider)
                 .OrderByDescending(x => x.AveragePricePerSquareMeter)
                  .Take(count)
                   .ToList();

            return districts;
        }
    }
}
