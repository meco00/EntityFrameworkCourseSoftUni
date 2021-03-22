using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstates.Datase;

namespace RealEstates.Services
{
   public class PropertiesService : IPropertiesService
    {
        public PropertiesService(RealEstateDBContext context)
        {

        }

        public void Add(string district, int floor, int maxFloor, int size, int year, string propertyType, string propertyTag)
        {
           
        }

        public IEnumerable<PropertyInfoDTO> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
           
        }
    }
}
