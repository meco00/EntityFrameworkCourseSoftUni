﻿using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
     public class SalesContext:DbContext
    {
        public SalesContext()
        {
                
        }
        public SalesContext(DbContextOptions options) :base(options)
        {
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SalesDb;Integrated Security=true");
            }


            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>().Property(x => x.Date).HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }



    }

}
