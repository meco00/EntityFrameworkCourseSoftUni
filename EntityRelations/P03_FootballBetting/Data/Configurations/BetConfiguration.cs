using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Configurations
{
    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> bet)
        {
            bet.HasKey(x => x.BetId);

            bet.Property(x => x.Prediction)
                .IsRequired()
                .IsUnicode(true);

            bet.HasOne(x => x.Game)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.GameId);

            bet.HasOne(x => x.User)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.UserId);




        }
    }
}
