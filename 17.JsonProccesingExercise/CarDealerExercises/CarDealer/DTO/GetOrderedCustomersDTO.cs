﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarDealer.DTO
{
   public class GetOrderedCustomersDTO
    {
       
        public string Name { get; set; }

       
        public string BirthDate { get; set; }

        
        public bool IsYoungDriver { get; set; }
    }
}
