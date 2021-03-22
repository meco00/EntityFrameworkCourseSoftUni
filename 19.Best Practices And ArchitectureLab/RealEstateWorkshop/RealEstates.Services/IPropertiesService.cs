using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        IEnumerable<PropertyInfoDTO> Search(int minPrice, int maxPrice, int minSize, int maxSize);
        

        
        void Add(string district,int floor,int maxFloor,int size,int year,string propertyType,string propertyTag );
    }
}
