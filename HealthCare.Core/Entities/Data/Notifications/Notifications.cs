using System;
using HealthCare.Core.AddRequest;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Entities.Data;

namespace HealthCare.Core.AddRequest
{
    public class Notification : AppEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}