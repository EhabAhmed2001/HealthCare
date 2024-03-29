using HealthCare.Core.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Configurations
{
    public class ServiceConfigurations : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.OwnsOne(S => S.Address, S => S.WithOwner());

            builder.ToTable("Service", "dbo");
        }
    }
}
