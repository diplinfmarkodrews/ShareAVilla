using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Threading.Tasks;
using System.Web.Mvc;
using ShareAVilla.Models;
using System.Security.Principal;

using Microsoft.AspNet.Identity.Owin;
namespace ShareAVilla.Controllers
{
    public static class ProfileManager
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private static ApplicationUser user;

        [Authorize]
        private static async Task<int> checkUserProfileIDandType(IPrincipal User)
        {

            try
            {

                userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                user = await userManager.FindByEmailAsync(User.Identity.Name);

                if (user == null)
                {
                    NotificationManager.AddException(new Exception("User Traveler not found"));
                    return -1;
                }
                if (user.ProfileID == -1)
                {
                    NotificationManager.AddException(new Exception("Userprofile ID is invalid"));
                    return -1;
                }
                if (string.Compare(user.UserType, "Traveler") == 0)
                {
                    return 1;
                }
                else
                {
                    if (string.Compare(user.UserType, "Villa") == 0)
                    {
                        return 1;
                    }
                }
                NotificationManager.AddException(new Exception("Userprofile String is invalid"));
                return -1;
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return -1;
            }
        }
       

        private static async Task<TravelerProfile> LoadTravelerProfile()
        {
            TravelerProfile profile = await db.TravelerProfile.FindAsync(user.ProfileID);
            if (profile == null)
            {
                Exception e = new Exception("Travelerprofile not found!");
                NotificationManager.AddException(e);

            }
            return profile;
        }
       
        public static async Task<TravelerProfile> LoadUserAndTravelerProfile(IPrincipal User)
        {
            try
            {
                await checkUserProfileIDandType(User);
                return await LoadTravelerProfile();
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return null;
            }

        }
       

    }
}

