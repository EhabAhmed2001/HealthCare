﻿using HealthCare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Configurations
{
    public class HistoryConfigurations : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.OwnsOne(H=>H.UserData, H => H.WithOwner());

            builder.ToTable("History", "dbo");

          /*  builder.Property(H => H.UserData.Temperature)
                .HasColumnType("decimal(18,2)");

            builder.Property(H => H.UserData.HeartRate)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(H => H.UserData.Oxygen)
                .HasColumnType("decimal(18,2)"); */
        }
    }
}
