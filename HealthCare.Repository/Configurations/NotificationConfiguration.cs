using HealthCare.Core.AddRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification", "dbo");

            builder.HasOne(N => N.Sender)
                .WithMany()
                .HasForeignKey(N => N.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(N => N.Receiver)
                .WithMany()
                .HasForeignKey(N => N.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
