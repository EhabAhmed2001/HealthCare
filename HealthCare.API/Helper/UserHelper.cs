﻿using HealthCare.Core.AddRequest;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.PL.Helper
{
    public static class UserHelper
    {
        // Method to search for a user by email and role
        public async static Task<AppUser?> UserSearch(string Email, string Role, UserManager<AppUser> userManager)
        {
            
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return null;
            }

            // Check if the user has the given role
           else if (await userManager.IsInRoleAsync(user, Role))
            {
                return user;
            }
            
          // If the user does not have the given role
            return null;
            
        }

        // Method to check if a notification exists
        public async static Task<Notification?> CheckIfNotificationExist(string SenderId, string ReceiverId, HealthCareContext _dbContext, NotificationStatus status = NotificationStatus.Pending)
        {
            var notification = await _dbContext.Notification.FirstOrDefaultAsync(n => ((n.SenderId == SenderId && n.ReceiverId == ReceiverId) || (n.SenderId == ReceiverId && n.ReceiverId == SenderId)) && n.Status == status);

            return notification;
        }
        // Method to add notification 
        public async static Task<bool> AddOrEditToNotificatiopn(string SenderId, string ReceiverId, string SenderEmail, string ReceiverEmail, HealthCareContext _dbcontext, NotificationStatus Status = NotificationStatus.Pending)
        {
            var notification = new Notification
            {
                SenderId = SenderId,
                ReceiverId = ReceiverId,
                SenderEmail = SenderEmail,
                ReceiverEmail = ReceiverEmail,
                Status = Status
            };

            _dbcontext.Notification.Add(notification);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        // Method to get patient data with his history by id
        public async static Task<Patient?> GetPatientData(string PatientId, HealthCareContext _dbcontext)
        {
            return await _dbcontext.Patient.Include(p => p.History).Include(P=>P.Doctor).FirstOrDefaultAsync(p => p.Id == PatientId);
        }

        // Method to get patient History with his history by id
        public async static Task<Patient?> GetPatientHistory(string PatientId, HealthCareContext _dbcontext)
        {
            return await _dbcontext.Patient.Include(p => p.History).Include(P => P.Doctor).FirstOrDefaultAsync(p => p.Id == PatientId);
        }
    }
}