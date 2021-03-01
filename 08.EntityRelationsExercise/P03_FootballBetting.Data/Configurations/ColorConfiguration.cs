using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;

using System.Text;

namespace P03_FootballBetting.Data.Configurations
{
   public class ColorConfiguration: IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> color)
        {
            color.Property(x => x.Name).
                IsRequired()
                .IsUnicode();

            color
                .HasMany(x => x.PrimaryKitTeams)
                .WithOne(x => x.PrimaryKitColor)
                .HasForeignKey(x => x.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            color
               .HasMany(x => x.SecondaryKitTeams)
               .WithOne(x => x.SecondaryKitColor)
               .HasForeignKey(x => x.SecondaryKitColorId)
               .OnDelete(DeleteBehavior.Restrict); 
        }

        
    }
}
