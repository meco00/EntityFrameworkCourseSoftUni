using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto
{
    [XmlType("Purchase")]
    public class PurchaseExportModel
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement("Cvc")]
        public string CvC { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Game")]
        public GameExportModel Game { get; set; }

    }

    
}