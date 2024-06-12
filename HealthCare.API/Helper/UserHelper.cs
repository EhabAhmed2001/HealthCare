using HealthCare.Core.AddRequest;
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
        public async static Task<bool> AddOrEditNotification(string SenderId, string ReceiverId, string SenderEmail, string ReceiverEmail, HealthCareContext _dbcontext, NotificationStatus Status = NotificationStatus.Pending)
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
        public async static Task<Patient?> GetPatientDataWithDoctor(string PatientId, HealthCareContext _dbcontext)
        {
            return await _dbcontext.Patient.Include(p => p.History).Include(P=>P.Doctor).FirstOrDefaultAsync(p => p.Id == PatientId);
        }
        
        public async static Task<Patient?> GetPatientDataWithObserver(string PatientEmail, HealthCareContext _dbcontext)
        {
            return await _dbcontext.Patient.Include(p => p.History).Include(P=>P.PatientObserver).FirstOrDefaultAsync(p => p.Email == PatientEmail);
        }

        public async static Task<Patient?> GetPatientDataWithDoctorAndObserver(string PatientId, HealthCareContext _dbcontext)
        {
            return await _dbcontext.Patient.Include(p => p.History).Include(P => P.Doctor).Include(P=>P.PatientObserver).FirstOrDefaultAsync(p => p.Id == PatientId);
        }

        // Insert File in wwwroot and return the path
        public static string UploadFile(IFormFile file, string folderName = "Images")
        {
            //1. Get Located Folder Path 
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            //2. Get File Name and Make it Unique 
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            //3. Get File Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);

            //4. Save File As Streams
            using var FS = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FS);

            //5. Return File Name
            return FilePath;
        }


        // Delete File from wwwroot
        public static void DeleteFile(string FilePath)
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}