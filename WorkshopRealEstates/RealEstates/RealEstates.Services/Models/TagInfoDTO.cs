using System.Xml.Serialization;

namespace RealEstates.Services.Models
{
    [XmlType("Tag")]
    public class TagInfoDTO
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}