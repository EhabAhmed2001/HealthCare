using HealthCare.Core.AddRequest;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.Data;
using HealthCare.Core.Entities.identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Data
{
    public class HealthCareContext : IdentityDbContext<AppUser>
    {
        public HealthCareContext(DbContextOptions<HealthCareContext> options) :base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<Patient>()
                .ToTable("Patient");

            modelBuilder.Entity<Doctor>()
                .ToTable("Doctor");

            modelBuilder.Entity<Observer>()
                .ToTable("Observer");

            base.OnModelCreating(modelBuilder);

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            base.ConfigureConventions(configurationBuilder);
        }

        // DbSets
        public DbSet<History> UserHistory { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Hardware> Hardware { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Observer> Observer { get; set; }
    }
}
