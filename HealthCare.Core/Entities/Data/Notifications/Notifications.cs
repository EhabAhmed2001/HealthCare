using System;
using HealthCare.Core.AddRequest;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthCare.Core.Entities.identity;

namespace HealthCare.Core.AddRequest
{
    public class Notification
    {
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }
        public int ReceiverId { get; set; }
        public AppUser Receiver { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    }
}