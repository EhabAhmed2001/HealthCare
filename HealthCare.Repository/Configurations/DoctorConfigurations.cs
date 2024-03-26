using HealthCare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Configurations
{
    public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasMany(D => D.History)
                .WithOne(H => H.Doctor)
                .HasForeignKey(H => H.HistoryDoctorId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
