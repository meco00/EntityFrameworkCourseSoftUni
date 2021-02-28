using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext:DbContext
    {
        public StudentSystemContext()
        {
                
        }

        public StudentSystemContext(DbContextOptions options):base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> HomeworkSubmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(x => new { x.StudentId, x.CourseId });
            });
            base.OnModelCreating(modelBuilder);
        }


    }
}
