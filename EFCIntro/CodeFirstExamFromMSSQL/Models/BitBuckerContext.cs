using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFirstExamFromMSSQL.Models
{
   public class BitBuckerContext:DbContext
    {
        public BitBuckerContext()
        {


        }

        public BitBuckerContext(DbContextOptions optionsBuilder)
            :base(optionsBuilder)
        {
                
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=BitBucker2;Integrated Security=true");
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RepositoriesContributors>()
                .HasKey(x => new { x.RepositoryId, x.ContributorId });

            modelBuilder.Entity<Commits>()
                   .Property(t => t.Message)
                   .IsRequired();

            modelBuilder.Entity<Files>()
                  .Property(t => t.Name)
                  .IsRequired();

            modelBuilder.Entity<Repositories>()
                 .Property(t => t.Name)
                 .IsRequired();


            modelBuilder.Entity<Commits>()
            .HasOne(p => p.User)
            .WithMany(b => b.Commits)
            .HasForeignKey(p => p.ContributorId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issues>()
           .HasOne(p => p.User)
           .WithMany(b => b.Issues)
           .HasForeignKey(p => p.AssigneeId);

            modelBuilder.Entity<Commits>()
        .HasOne(p => p.Issue)
        .WithMany(b => b.Commits)
        .IsRequired(false);

            modelBuilder.Entity<RepositoriesContributors>()
          .HasOne(p => p.User)
          .WithMany(b => b.RepositoriesContributors)
          .HasForeignKey(p => p.ContributorId);

        

            modelBuilder.Entity<Commits>()
                .HasOne(p => p.Repository)
                .WithMany(b => b.Commits)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);




        }

        public DbSet<Files> Files { get; set; }

        public DbSet<Commits> Commits { get; set; }

        public DbSet<Issues> Issues { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Repositories> Repositories { get; set; }

        public DbSet<RepositoriesContributors> RepositoriesContributors { get; set; }
    }
}
