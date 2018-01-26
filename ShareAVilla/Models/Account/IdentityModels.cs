using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ShareAVilla.Models
{
 
    // Sie können Profildaten für den Benutzer durch Hinzufügen weiterer Eigenschaften zur ApplicationUser-Klasse hinzufügen. Weitere Informationen finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=317594".
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen            
            return userIdentity;
        }
        public string UserType { get; set; }

        public int ProfileID { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        //Booking not neccessary yet!!!
       // public DbSet<Reservation.Reservation> Reservation { get; set; }
        //public DbSet<VillaReservationTime> VillaReservationTime { get; set; }
        //Mailbox
        public DbSet<Attachement> Attachement { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<Mailbox> Mailbox { get; set; }
        //Profile
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<TravelerProfile> TravelerProfile { get; set; }
        public DbSet<TravelerProfileDescription> TravelerProfileDescription { get; set; }
        public DbSet<TravelerProfileAttributes> TravelerProfileAttributes { get; set; }    
        public DbSet<TravelerProfileReviews> TravelerReviews { get; set; }
        //Request
        public DbSet<Request.Request> Requests { get; set; }
        public DbSet<Accommodation.Accommodation> Accommodations { get; set; }
        public DbSet<Accommodation.AccommodationProfile> AccommProfile { get; set; }
        public DbSet<Accommodation.BedRoom> BedRooms { get; set; }
        public DbSet<Accommodation.Likibu.Offer> LikibuOffer { get; set; }
        public DbSet<RoomRequest.RoomRequest> RoomRequests { get; set; }
        public DbSet<RoomRequest.RoomRequestResponse> RoomRequestResponse { get; set; }
        //public DbSet<Request.RequestTime> RequestTime { get; set; } //might be needed later ...
        public DbSet<TravelerProfileSearchSession> LikibuSearchSession { get; set; }
        public DbSet<Find.Search> Searches { get; set; }
        
        //public DbSet<Review> VillaReview { get; set; }
        // public DbSet<Elmah.Error> ElmahErr { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Review> Reviews { get; set; }

        public DbSet<Find.FilterProperties> FilterProperties { get; set; }

//        public DbSet<Request.RequestVMOwner> RequestVMOwners { get; set; }

        public DbSet<VM.ProfilePreviewVM> ProfilePreviewVMs { get; set; }

        public DbSet<Find.SearchVM> SearchVMs { get; set; }

       // public DbSet<RoomRequest.RoomRequestVMUser> RoomRequestVMUsers { get; set; }

       // public DbSet<RoomRequest.RoomRequestVMOwner> RoomRequestVMOwners { get; set; }

        public DbSet<Accommodation.BedRoomVM> BedRoomVMs { get; set; }

        public System.Data.Entity.DbSet<ShareAVilla.Models.VM.ReviewVM> ReviewVMs { get; set; }

        public System.Data.Entity.DbSet<ShareAVilla.Models.RoomRequest.RoomRequestVMUser> RoomRequestVMUsers { get; set; }

        public System.Data.Entity.DbSet<ShareAVilla.Models.Request.RequestVMListing> RequestVMListings { get; set; }

        public System.Data.Entity.DbSet<ShareAVilla.Models.RoomRequest.RoomRequestVMOwner> RoomRequestVMOwners { get; set; }
    }
}