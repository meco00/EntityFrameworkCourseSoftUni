using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto
{
    [XmlType("Game")]
    public class GameExportModel
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Genre")]
        public string GenreName { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }




    }

    // <Game title = "Counter-Strike: Global Offensive" >
    //////      < Genre > Action </ Genre >
    //////      < Price > 12.49 </ Price >
}