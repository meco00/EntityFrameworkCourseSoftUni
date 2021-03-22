using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Import
{
    [XmlType("Sale")]
   public class SaleInputModel
    {
        // <Sale>
        //<carId>234</carId>
        //<customerId>23</customerId>
        //<discount>50</discount>

        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }
    }
}
