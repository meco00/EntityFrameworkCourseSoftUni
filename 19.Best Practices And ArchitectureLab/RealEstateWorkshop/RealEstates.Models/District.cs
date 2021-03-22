using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Models
{
   public class District
    {
        public District()
        {
            this.PropertyAds = new HashSet<PropertyAd>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PropertyAd> PropertyAds { get; set; }
    }
}
