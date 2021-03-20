using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{

   public class ProductWithCountOutputModel
    {

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ProductOutputModel[] ProductOutputModels { get; set; }
    }
}
