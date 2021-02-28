using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityRelationsLab.Veterinary
{
    public class VeterinaryContext:DbContext
    {

        public DbSet<Person> Persons { get; set; }

        public DbSet<Pet> Pets { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Veterinary;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Property(x => x.FirstName).IsRequired();
            modelBuilder.Entity<Person>().Property(x => x.LastName).IsRequired();

            modelBuilder.Entity<Person>()
                .Property(x => x.LastUpdated)
                .HasDefaultValue(DateTime.UtcNow)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Pet>().Property(x => x.Type).IsRequired();
            modelBuilder.Entity<Pet>().HasIndex(x => x.Type).IsUnique();


            modelBuilder.Entity<Person>()
                .HasMany(x => x.Pets)
                .WithOne(x => x.Parent)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
