using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> country)
        {
            country.Property(x => x.Name).
               IsRequired()
               .IsUnicode();

            country.HasMany(x => x.Towns)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId);
        }
    }
}
