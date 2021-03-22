using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    //[XmlType("Users")]
   public class UsersWithProductsOuputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }


        [XmlArray("users")]
        public SoldUsersProductOutputModel[] SoldUsersProductOutputModel { get; set; }
    }
}
