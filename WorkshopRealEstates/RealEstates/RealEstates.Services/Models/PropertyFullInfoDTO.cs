using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RealEstates.Services.Models
{
    [XmlType("Property")]
   public class PropertyFullInfoDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("District")]
        public string DistrictName { get; set; }

        [XmlElement("Size")]
        public int Size { get; set; }

        [XmlElement("Price")]
        public int Price { get; set; }

        [XmlElement("Property")]
        public string PropertyType { get; set; }

        [XmlElement("Building")]
        public string BuildingType { get; set; }

        [XmlArray("Tags")]
        public TagInfoDTO[] Tags { get; set; }
    }
}
