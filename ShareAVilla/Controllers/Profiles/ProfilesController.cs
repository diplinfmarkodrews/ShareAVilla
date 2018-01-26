using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareAVilla.Models;
using System.Security.Principal;
using System.Drawing.Imaging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;



namespace ShareAVilla.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        
        private ApplicationUserManager userManager;
        private ApplicationUser user;

        public static async Task<TravelerProfile> LoadProfile(int id)
        {
            
            using (var db = new ApplicationDbContext())
            {

                TravelerProfile tp = await db.TravelerProfile.FindAsync(id);
                tp.Description = await db.TravelerProfileDescription.FindAsync(tp.TravelerProfileDescriptionID);
                tp.Description.ProfilePic = await db.Pictures.FindAsync(tp.Description.ProfilePic_ID);
                tp.Attributes = await db.TravelerProfileAttributes.FindAsync(tp.TravelerAttributesID);
                tp.Reviews = await db.TravelerReviews.FindAsync(tp.TravelerReviewsID);//.Include(p=>p.Reviews).FirstOrDefault();
                tp.Reviews.Reviews = await db.Reviews.Where(p => p.ProfileReviewsID==tp.TravelerReviewsID).ToListAsync();     
                   
                return tp;
            }
           
            
           


        }

        public static Models.VM.TravelerProfileVM LoadTravelerVM(TravelerProfile tp)
        {
            Models.VM.TravelerProfileVM profileVM = new Models.VM.TravelerProfileVM(tp);

            
            return profileVM;
        }


        // GET: TravelerProfiles
        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (User!=null)
            {
                ///Load TravelerInfos Mailbox, Requests, RoomRequests

                TravelerProfile profile = await ProfileManager.LoadUserAndTravelerProfile(User);
                if (profile!=null)
                {
                    profile = await LoadProfile(profile.ID);
                    Models.VM.TravelerProfileVM profileVM = new Models.VM.TravelerProfileVM(profile);

                    return View(profileVM);
                }
            }

            return RedirectToAction("Create");

        }

        // GET: TravelerProfiles/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                TravelerProfile tp;
                TravelerProfile tpcheck;
                bool user = false;
                if (id == null) /// load own ProfileDetails
                {
                    tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    tp = await LoadProfile(tp.ID);
                    user = true;
                }
                else
                {
                    
                    tp = await LoadProfile(id.Value);
                    tpcheck = await ProfileManager.LoadUserAndTravelerProfile(User);
                    if (tp.ID == tpcheck.ID)
                    {
                        user = true;
                    }
                }
                
               
                Models.VM.TravelerProfileVM profileVM = LoadTravelerVM(tp);
                int i = 0;
                foreach (var rev in tp.Reviews.Reviews)
                {
                    
                    Models.VM.ProfilePreviewVM poster = await ProfilePreviewVM(rev.TravelerPoster_ID);
                    profileVM.ReviewsVM.Reviews[i].Poster = poster;
                    i++;
                }                
                profileVM.User = user;
                if (!user)
                {
                    profileVM.createReview = new Models.VM.CreateReviewVM(id.Value);
                }

                return View(profileVM);
                                               
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return RedirectToAction("Index", "Home");    
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _PostReview(Models.VM.CreateReviewVM review)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TravelerProfile tpcheck = await ProfileManager.LoadUserAndTravelerProfile(User);
                    TravelerProfile tp = await ProfilesController.LoadProfile(review.ID);
                    if (tpcheck.ID == tp.ID)
                    {
                        throw new Exception("Access Violation Post Review! User tried to Review his own profile", new AccessViolationException());
                    }
                    TravelerProfileReviews treviews = tp.Reviews; 
                    var tpRevUpdate = treviews;
                    TravelerProfile tPoster = await ProfileManager.LoadUserAndTravelerProfile(User);
                    Review rev = new Review(review, tPoster.ID, tp.TravelerReviewsID);
                    tpRevUpdate.Reviews.Add(rev);
                    using (var db = new ApplicationDbContext())
                    {
                        db.Reviews.Add(rev);
                        
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");                    
                }

            }
            return RedirectToAction("Details", new { id = review.ID });
        }
        // GET: TravelerProfiles/Create
        [Authorize]
        public ActionResult Create()
        {
             Models.VM.TravelerProfileVM travelerProfileVM = new Models.VM.TravelerProfileVM();

            return View(travelerProfileVM);
        }
        
        // POST: TravelerProfiles/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
     

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.VM.TravelerProfileVM travelerProfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    user = await userManager.FindByEmailAsync(User.Identity.Name);
                    TravelerProfile profile = new TravelerProfile();
                    PopulateTavelerProfile(profile);
                    profile.SetValues(travelerProfile);
                    Picture pic = ImageProcessing.ReadPicture(travelerProfile.DescriptionVM.upload);
                    if (pic != null)
                    {
                        profile.Description.ProfilePic = pic;
                    }
                    else
                    {
                        profile.Description.ProfilePic = ImageProcessing.ReadPicture(Server.MapPath("~/Images/ProfileBlankPic.png"));
                    }
                    
                    profile.TimeStamp = DateTime.Now;
                    using (var db = new ApplicationDbContext())
                    {
                        db.TravelerProfile.Add(profile);
                        await db.SaveChangesAsync();
                    }
                    user.ProfileID = profile.ID;
                    user.UserType = "Traveler";
                    await userManager.UpdateAsync(user);
                    NotificationManager.AddNotification(NotificationType.Success, "Travelerprofile successful created");
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    return View(travelerProfile);
                }
            }
            
            return View(travelerProfile);
        }
        private void PopulateTavelerProfile(TravelerProfile travelerProfile)
        {
            travelerProfile.Description = new TravelerProfileDescription();
            travelerProfile.Description.ProfilePic = new Picture();
            travelerProfile.Attributes = new TravelerProfileAttributes();

            travelerProfile.Attributes.Language = "en";
            travelerProfile.Gallery = new Gallery();
            travelerProfile.Mailbox = new Mailbox();

            travelerProfile.Reviews = new TravelerProfileReviews();
            travelerProfile.SearchSession = new TravelerProfileSearchSession();
        }
        // GET: TravelerProfiles/Edit/5 
        public static async Task<Models.VM.ProfilePreviewVM> ProfilePreviewVM(int id)
        {
            TravelerProfile tp = await ProfilesController.LoadProfile(id);
            float score = 0.0f;
            if (tp.Reviews.Reviews.Count > 0)
            {
                foreach (var rev in tp.Reviews.Reviews)
                {
                    score = score + rev.Ranking;
                }
                score = score / tp.Reviews.Reviews.Count;
            }
            tp.Reviews.Score = score;
            Models.VM.ProfilePreviewVM tpPrev = new Models.VM.ProfilePreviewVM(tp);
            return tpPrev;
        }
        public static string Currency(string cur)
        {
            switch (cur)
            {
                case "usd": return "$";
                case "eur": return "€";
                default: return "$";
            }
        }
        public static async Task<string> GetCurrency(IPrincipal User)
        {
            if (User != null)
            {
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                tp = await LoadProfile(tp.ID);
                return Currency(tp.Attributes.Currency);
            }
            else return "$";
        }
        public async Task<ActionResult> ShowProfileLogo(int? id)
        {
            using (var db = new ApplicationDbContext()) {
                //TravelerProfile tp = await db.TravelerProfileDescrption.


                    }
            return PartialView();
        }


        [Authorize]
        public async Task<ActionResult> Edit()
        {


            try
            {

                TravelerProfile profile = await ProfileManager.LoadUserAndTravelerProfile(User);

                if (profile!=null)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        profile = await LoadProfile(profile.ID);

                        Models.VM.TravelerProfileVM profileVM = new Models.VM.TravelerProfileVM(profile);

                        return View(profileVM);
                    }
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: TravelerProfiles/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.VM.TravelerProfileVM tpVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    if (tp!=null)
                    {
                        
                        using (var db = new ApplicationDbContext())
                        {
                            tp = await LoadProfile(tp.ID);//tp is the updated version
                            TravelerProfile travelerDB = db.TravelerProfile.Find(tp.ID);
                            var Attributes = await db.TravelerProfileAttributes.FindAsync(tp.TravelerAttributesID);
                            var Description = await db.TravelerProfileDescription.FindAsync(tp.TravelerProfileDescriptionID);
                            var ProfilePic = await db.Pictures.FindAsync(tp.Description.ProfilePic_ID);

                            tp.SetValues(tpVM); //setting old values w/o picture


                            //db.Entry(travelerDB).CurrentValues.SetValues(tp);
                            tp.Attributes.ID = Attributes.ID;
                            db.Entry(Attributes).CurrentValues.SetValues(tp.Attributes);

                            if (ProfilePic != null) /// old picture exists           ///Check tp here!! IDs
                            {
                                if (tpVM.DescriptionVM.upload != null) //new upload exists
                                {

                                    //overwrite old Pic
                                    
                                   
                                    Picture newPic = ImageProcessing.ReadPicture(tpVM.DescriptionVM.upload);

                                    newPic.ID = ProfilePic.ID; //Setting id of old pic id
                                    tp.Description.ProfilePic_ID = newPic.ID;
                                    tp.Description.ProfilePic = newPic;

                                    db.Entry(ProfilePic).CurrentValues.SetValues(tp.Description.ProfilePic);
                                    db.Entry(Description).CurrentValues.SetValues(tp.Description); //kann denn raus
                                }
                                //do nothing with Pic
                                tp.Description.ID = Description.ID;
                                db.Entry(Description).CurrentValues.SetValues(tp.Description);
                            }
                            else //no old pic exists
                            {
                                if (tpVM.DescriptionVM.upload != null) //new upload exists
                                {
                                    tp.Description.ProfilePic = ImageProcessing.ReadPicture(tpVM.DescriptionVM.upload);
                                    db.Pictures.Add(tp.Description.ProfilePic);
                                    tp.Description.ID = Description.ID;
                                    db.Entry(Description).CurrentValues.SetValues(tp.Description);
                                }
                                tp.Description.ID = Description.ID;
                                db.Entry(Description).CurrentValues.SetValues(tp.Description);
                            }                                                                                   
                            await db.SaveChangesAsync();                          
                                                        
                        }
                        NotificationManager.AddNotification(NotificationType.Success, "Profile update successful");
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                }
            }
            return View(tpVM);
        }

        // GET: TravelerProfiles/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new ApplicationDbContext())
            {
                TravelerProfile travelerProfile = await db.TravelerProfile.FindAsync(id);
                if (travelerProfile == null)
                {
                    return HttpNotFound();
                }
                return View(travelerProfile);
            }
        }

        // POST: TravelerProfiles/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                TravelerProfile travelerProfile = await db.TravelerProfile.FindAsync(id);
                db.TravelerProfile.Remove(travelerProfile);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
