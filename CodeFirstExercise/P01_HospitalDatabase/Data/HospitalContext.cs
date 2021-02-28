using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext()
        {

        }

        public HospitalContext(DbContextOptions options) :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=HospitalDb;Integrated Security=true;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>(x =>
            {
                x.HasKey(c => new { c.PatientId, c.MedicamentId });
            });


            base.OnModelCreating(modelBuilder);
        }

    }
}
