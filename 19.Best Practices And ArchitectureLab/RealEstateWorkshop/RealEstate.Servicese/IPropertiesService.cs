using RealEstates.Servicese.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Servicese
{
    public interface IPropertiesService
    {
        IEnumerable<PropertyInfoDTO> Search(int minPrice, int maxPrice, int minSize, int maxSize);

        public decimal AveragePricePerSquareMeter();


        void Add(string district, int price,
            int floor, int maxFloor, int size, int yardSize,
            int year, string propertyType, string buildingType);
    }
}
