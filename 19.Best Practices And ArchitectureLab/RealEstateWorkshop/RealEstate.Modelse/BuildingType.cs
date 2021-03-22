using System.Collections.Generic;


namespace RealEstates.Modelse
{
    public class BuildingType
    {
        public BuildingType()
        {
            this.PropertyAds = new HashSet<PropertyAd>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PropertyAd> PropertyAds { get; set; }
    }
}