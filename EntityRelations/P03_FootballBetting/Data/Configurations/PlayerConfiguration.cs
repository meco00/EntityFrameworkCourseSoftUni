using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.SquadNumber).IsRequired();

            builder.HasOne(x => x.Position)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.PositionId);

            builder.HasMany(x => x.PlayerStatistics)
                .WithOne(x => x.Player).
                HasForeignKey(x => x.PlayerId);
        }
    }
}
