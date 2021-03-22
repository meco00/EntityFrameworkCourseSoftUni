using Microsoft.EntityFrameworkCore;

using RealEstates.Modelse;
using System;

namespace RealEstates.Datase
{
    public class RealEstateDBContext : DbContext
    {
        public RealEstateDBContext()
        {

        }

        public RealEstateDBContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<PropertyAd> PropertyAds { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PropertyTag> PropertyTags { get; set; }

        public DbSet<Modelse.Type> Types { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=RealEstates;Integrated Security=true");
            }

        }
    }
}
