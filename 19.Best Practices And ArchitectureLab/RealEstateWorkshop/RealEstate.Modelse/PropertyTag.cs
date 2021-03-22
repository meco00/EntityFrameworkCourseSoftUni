using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Modelse
{
   public class PropertyTag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PropertyAd> PropertyAds { get; set; }
    }
}
