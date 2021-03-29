﻿using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
  public  class BookInputModel
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(30,MinimumLength =3)]
        public string Name { get; set; }

        [XmlElement("Genre")]
        [Required]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }

        [XmlElement("Price")]
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [XmlElement("Pages")]
        [Range(typeof(int),"50","5000")]
        public int Pages { get; set; }

        [XmlElement("PublishedOn")]
        [Required]
        public string PublishedOn { get; set; }
    }
   
}
