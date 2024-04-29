using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Configurations
{
    public class PatientConfigurations : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasOne(P => P.PatientObserver)
                .WithOne(O => O.Patient)
                .HasForeignKey<Observer>(O => O.PatientObserverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(P => P.Doctor)
                .WithMany(D => D.Patients)
                .HasForeignKey(P => P.PatientDoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(P => P.History)
                .WithOne(H => H.Patient)
                .HasForeignKey(H => H.HistoryPatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Hardware)
                .WithOne()
                .HasForeignKey<Patient>(p => p.HardwareId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasIndex(p => p.HardwareId)
                .IsUnique();
        }
    }
}
