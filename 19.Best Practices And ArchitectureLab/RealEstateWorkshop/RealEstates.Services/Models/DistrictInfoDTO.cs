﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services.Models
{
   public class DistrictInfoDTO
    {
        public string Name { get; set; }

        public decimal AveragePricePerSquareKilometer { get; set; }

        public int PropertiesCount { get; set; }
    }
}
