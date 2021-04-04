using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public class TaskExportXmlModel
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string Label { get; set; }
    }

    //< Task >
    ////    < Name > Broadleaf </ Name >
    ////    < Label > JavaAdvanced </ Label >
}