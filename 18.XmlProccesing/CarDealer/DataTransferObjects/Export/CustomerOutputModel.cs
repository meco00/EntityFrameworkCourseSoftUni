using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Export
{
    [XmlType("customer")]
   public class CustomerOutputModel
    {
        //      <customers>
        //<customer full-name="Hai Everton" bought-cars="1" spent-money="2544.67" />

        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCarsCount { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }

    }
}
