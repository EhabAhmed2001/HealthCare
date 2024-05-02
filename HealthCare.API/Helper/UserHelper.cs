using HealthCare.Core.AddRequest;
using HealthCare.Core.Entities.identity;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.PL.Helper
{
    public static class UserHelper
    {
        // Method to search for a user by email and role
        public async static Task<string?> UserSearch(string Email, string Role, UserManager<AppUser> userManager)
        {
            
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return null;
            }

            // Check if the user has the given role
           else if (await userManager.IsInRoleAsync(user, Role))
            {
                return user.Id;
            }
            
          // If the user does not have the given role
            return null;
            
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
    }
}
