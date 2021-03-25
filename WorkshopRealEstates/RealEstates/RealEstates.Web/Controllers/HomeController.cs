using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstates.Services;
using RealEstates.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstates.Web.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IDistrictsService districtsService;

        public HomeController(IDistrictsService districtsService)
        {
            
            this.districtsService = districtsService;
        }

        public IActionResult Index()
        {
           var mostExpensiveDistricts = this.districtsService.GetMostExpensiveDistricts(10);
            return View(mostExpensiveDistricts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
