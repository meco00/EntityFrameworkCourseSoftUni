﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
     public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } 
            = new HashSet<Sale>();


    }
}
