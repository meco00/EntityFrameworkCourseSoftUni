using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Modelse
{
    public class PropertyAd
    {
        //    "Url": "https://www.imot.bg/pcgi/imot.cgi?act=5&adv=1h161401019485607&slink=6hs7tg&f1=1",
        //"Size": 10,
        //"YardSize": 0,
        //"Floor": 1,
        //"TotalFloors": 4,
        //"District": "град София, Център",
        //"Year": 0,
        //"Type": "АТЕЛИЕ, ТАВАН",
        //"BuildingType": "Тухла",
        //"Price": 20000

        public int Id { get; set; }

        public string Url { get; set; }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public byte? Floor { get; set; }

        public byte? TotalFloors { get; set; }

        public int DistrictId { get; set; }

        public virtual District District { get; set; }

        public int? Year { get; set; }

        public int TypeId { get; set; }

        public  virtual Type Type { get; set; }

        public int BuildingTypeId { get; set; }

        public virtual BuildingType BuildingType { get; set; }

        //Price is in EUR
        public int? Price { get; set; }

        public virtual ICollection<PropertyTag> PropertyTags { get; set; }

















    }
}
