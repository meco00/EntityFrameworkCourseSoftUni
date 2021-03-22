using ProductShop.Dtos.Export;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    [XmlType("User")]
   public class SoldUsersProductOutputModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public ProductWithCountOutputModel SoldProducts { get; set; }



    }
}
