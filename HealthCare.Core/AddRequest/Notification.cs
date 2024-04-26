using System;
using HealthCare.Core.AddRequest;
using HealthCare.Core.Entities.identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Core.AddRequest
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Notification> Notifications { get; set; }

        public class Notification
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public AppUser Sender { get; set; }
            public int ReceiverId { get; set; }
            public AppUser Receiver { get; set; }
            public string SenderEmail { get; set; }
            public string ReceiverEmail { get; set; }
            public NotificationStatus Status { get; set; }
        }
    }
}