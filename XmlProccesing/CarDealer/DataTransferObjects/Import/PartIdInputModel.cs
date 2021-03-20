using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Import
{
    [XmlType("partId")]
    public class PartIdInputModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}