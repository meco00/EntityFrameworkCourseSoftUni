﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
   public class Resource
    {
        public int ResourceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(250)")]
        public string URL { get; set; }


        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }
        public  virtual Course Course { get; set; }


    }
}
