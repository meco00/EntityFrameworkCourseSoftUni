using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Export
{
    [XmlType("car")]
   public class CarsWithPartsOutputModel
    {
        //      <cars>
        //<car make = "Opel" model="Astra" travelled-distance="516628215">
        //  <parts>
        //    <part name = "Master cylinder" price="130.99" />

        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }


        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public PartsOutpuModel[] Parts { get; set; }

    }
}
