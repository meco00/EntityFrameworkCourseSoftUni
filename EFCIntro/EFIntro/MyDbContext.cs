using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFIntro
{
     public class MyDbContext:DbContext
    {
        public MyDbContext()
        {

        }

        public MyDbContext(DbContextOptions optionsBuilder)
            :base(optionsBuilder)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SliDo;Integrated Security=true");
            }
           
        }




        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                 .HasIndex(u => u.Name)
                 .IsUnique();

            modelBuilder.Entity<Category>().Property(x => x.Name).IsUnicode(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
