using Microsoft.AspNetCore.Mvc;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstates.Web.Controllers
{
    public class PropertiesController:Controller
    {
        private readonly IPropertiesService propertiesService;

        public PropertiesController(IPropertiesService propertiesService)
        {
            this.propertiesService = propertiesService;
        }

        public IActionResult Search()
        {
            return this.View();
        }
        //minPrice=1&maxPrice=2&minSize=3&maxSize=4
        public IActionResult DoSearch(int minPrice,int maxPrice,int minSize,int maxSize)
        {
           var searchedProperties= this.propertiesService.Search(minPrice, maxPrice, minSize, maxSize);
            return this.View(searchedProperties);
        }
    }
}
