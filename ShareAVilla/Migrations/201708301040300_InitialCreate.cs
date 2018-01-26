namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Mail_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Mails", t => t.Mail_ID)
                .Index(t => t.Mail_ID);
            
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProfilePic_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pictures", t => t.ProfilePic_ID)
                .Index(t => t.ProfilePic_ID);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        FileName = c.String(),
                        Gallery_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Galleries", t => t.Gallery_ID)
                .Index(t => t.Gallery_ID);
            
            CreateTable(
                "dbo.SearchSessions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SearchID = c.String(),
                        Location = c.String(),
                        CheckIn = c.DateTime(nullable: false),
                        CheckOut = c.DateTime(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Mails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Text = c.String(),
                        isRecieved = c.Boolean(nullable: false),
                        isRead = c.Boolean(nullable: false),
                        isAnswered = c.Boolean(nullable: false),
                        Mailbox_ID = c.Int(),
                        Mailbox_ID1 = c.Int(),
                        Mailbox_ID2 = c.Int(),
                        Mailbox_ID3 = c.Int(),
                        Sender_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Mailboxes", t => t.Mailbox_ID)
                .ForeignKey("dbo.Mailboxes", t => t.Mailbox_ID1)
                .ForeignKey("dbo.Mailboxes", t => t.Mailbox_ID2)
                .ForeignKey("dbo.Mailboxes", t => t.Mailbox_ID3)
                .ForeignKey("dbo.Mailboxes", t => t.Sender_ID)
                .Index(t => t.Mailbox_ID)
                .Index(t => t.Mailbox_ID1)
                .Index(t => t.Mailbox_ID2)
                .Index(t => t.Mailbox_ID3)
                .Index(t => t.Sender_ID);
            
            CreateTable(
                "dbo.Mailboxes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        hasNewMail = c.Boolean(nullable: false),
                        Mail_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Mails", t => t.Mail_ID)
                .Index(t => t.Mail_ID);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProfileAdress = c.String(),
                        PricePerRoom = c.Double(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                        Location = c.String(),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        CheckIn = c.DateTime(nullable: false),
                        CheckOut = c.DateTime(nullable: false),
                        Title = c.String(),
                        Text = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        AccomProfile_ID = c.Int(),
                        VillaRequests_ID = c.Int(),
                        AvailableRooms_ID = c.Int(),
                        RequestTraveler_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccomProfiles", t => t.AccomProfile_ID)
                .ForeignKey("dbo.VillaRequests", t => t.VillaRequests_ID)
                .ForeignKey("dbo.RoomsAvailables", t => t.AvailableRooms_ID)
                .ForeignKey("dbo.RoomRequests", t => t.RequestTraveler_ID)
                .Index(t => t.AccomProfile_ID)
                .Index(t => t.VillaRequests_ID)
                .Index(t => t.AvailableRooms_ID)
                .Index(t => t.RequestTraveler_ID);
            
            CreateTable(
                "dbo.AccomProfiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BedRooms = c.Int(nullable: false),
                        Bathrooms = c.Int(nullable: false),
                        Adress = c.String(),
                        Region = c.String(),
                        LongLat = c.String(),
                        WebAdress = c.String(),
                        Price = c.Double(nullable: false),
                        HasKitchen = c.Boolean(nullable: false),
                        HasPool = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RoomRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestedBeds = c.Int(nullable: false),
                        Prize = c.Single(nullable: false),
                        Text = c.String(),
                        isApproved4Booking = c.Boolean(nullable: false),
                        RequestedTime_ID = c.Int(),
                        RequestingTraveler_ID = c.Int(),
                        Request_ID = c.Int(),
                        Request_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RequestTimes", t => t.RequestedTime_ID)
                .ForeignKey("dbo.TravelerProfiles", t => t.RequestingTraveler_ID)
                .ForeignKey("dbo.Requests", t => t.Request_ID)
                .ForeignKey("dbo.Requests", t => t.Request_ID1)
                .Index(t => t.RequestedTime_ID)
                .Index(t => t.RequestingTraveler_ID)
                .Index(t => t.Request_ID)
                .Index(t => t.Request_ID1);
            
            CreateTable(
                "dbo.BedRooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SingleBed = c.Int(nullable: false),
                        DoubleBed = c.Int(nullable: false),
                        RoomRequest_ID = c.Int(),
                        RoomsAvailable_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RoomRequests", t => t.RoomRequest_ID)
                .ForeignKey("dbo.RoomsAvailables", t => t.RoomsAvailable_ID)
                .Index(t => t.RoomRequest_ID)
                .Index(t => t.RoomsAvailable_ID);
            
            CreateTable(
                "dbo.RequestTimes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartTime = c.String(),
                        EndTime = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TravelerProfiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TravelerProfileDescriptionID = c.Int(nullable: false),
                        TravelerAttributesID = c.Int(nullable: false),
                        TravelerMailboxID = c.Int(nullable: false),
                        TravelerGalleryID = c.Int(nullable: false),
                        VillaRequestsID = c.Int(nullable: false),
                        ReservedVillasID = c.Int(nullable: false),
                        Reservation_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TravelerProfileAttributes", t => t.TravelerAttributesID, cascadeDelete: true)
                .ForeignKey("dbo.TravelerProfileDescriptions", t => t.TravelerProfileDescriptionID, cascadeDelete: true)
                .ForeignKey("dbo.Galleries", t => t.TravelerGalleryID, cascadeDelete: true)
                .ForeignKey("dbo.Mailboxes", t => t.TravelerMailboxID, cascadeDelete: true)
                .ForeignKey("dbo.VillaRequests", t => t.VillaRequestsID, cascadeDelete: true)
                .ForeignKey("dbo.Reservations", t => t.Reservation_ID)
                .ForeignKey("dbo.VillaReservations", t => t.ReservedVillasID, cascadeDelete: true)
                .Index(t => t.TravelerProfileDescriptionID)
                .Index(t => t.TravelerAttributesID)
                .Index(t => t.TravelerMailboxID)
                .Index(t => t.TravelerGalleryID)
                .Index(t => t.VillaRequestsID)
                .Index(t => t.ReservedVillasID)
                .Index(t => t.Reservation_ID);
            
            CreateTable(
                "dbo.TravelerProfileAttributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NumPerson = c.Int(nullable: false),
                        IsSmoking = c.Boolean(nullable: false),
                        NoiseLevel = c.Int(nullable: false),
                        HasPets = c.Boolean(nullable: false),
                        IsVegetarian = c.Boolean(nullable: false),
                        Language = c.String(),
                        Currency = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TravelerProfileDescriptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Age = c.String(nullable: false),
                        Sex = c.String(nullable: false),
                        Nationality = c.String(),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VillaRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VillaReservations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReservationTime_ID = c.Int(),
                        ReservingTraveler_ID = c.Int(),
                        VillaReservations_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.VillaReservationTimes", t => t.ReservationTime_ID)
                .ForeignKey("dbo.TravelerProfiles", t => t.ReservingTraveler_ID)
                .ForeignKey("dbo.VillaReservations", t => t.VillaReservations_ID)
                .Index(t => t.ReservationTime_ID)
                .Index(t => t.ReservingTraveler_ID)
                .Index(t => t.VillaReservations_ID);
            
            CreateTable(
                "dbo.VillaReservationTimes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.String(),
                        EndDate = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                        Ranking = c.Int(nullable: false),
                        Poster_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TravelerProfiles", t => t.Poster_ID)
                .Index(t => t.Poster_ID);
            
            CreateTable(
                "dbo.RoomsAvailables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AvailableBeds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Searches",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestType = c.Int(nullable: false),
                        Location = c.String(),
                        CheckIn = c.DateTime(nullable: false),
                        CheckOut = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchVMs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SearchID = c.String(),
                        SearchStatus = c.String(),
                        FilterProps_ID = c.Int(),
                        LikibuResults_ID = c.String(maxLength: 128),
                        Search_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FilterProperties", t => t.FilterProps_ID)
                .ForeignKey("dbo.SearchRequestResults", t => t.LikibuResults_ID)
                .ForeignKey("dbo.Searches", t => t.Search_ID)
                .Index(t => t.FilterProps_ID)
                .Index(t => t.LikibuResults_ID)
                .Index(t => t.Search_ID);
            
            CreateTable(
                "dbo.FilterProperties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PerPage = c.Int(nullable: false),
                        Page = c.Int(nullable: false),
                        Sort = c.String(),
                        PrivacyType = c.String(),
                        AccomType = c.String(),
                        Amenities = c.String(),
                        PartnerID = c.Int(nullable: false),
                        Guests = c.Int(nullable: false),
                        PriceMin = c.Int(nullable: false),
                        PriceMax = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchRequestResults",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Status = c.String(),
                        Bbox = c.String(),
                        Where = c.String(),
                        CheckIn = c.DateTime(nullable: false),
                        CheckOut = c.DateTime(nullable: false),
                        CountryCode = c.String(),
                        TotalResults = c.Int(nullable: false),
                        TotalPages = c.Int(nullable: false),
                        MinPrice = c.Int(nullable: false),
                        MaxPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        URL = c.String(),
                        PriceTotal = c.Int(nullable: false),
                        PriceNight = c.Int(nullable: false),
                        Currency = c.String(),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        AverageRating = c.Double(nullable: false),
                        RatingCount = c.Int(nullable: false),
                        MaxGuests = c.Int(nullable: false),
                        BedRooms = c.Int(nullable: false),
                        BathRooms = c.Int(nullable: false),
                        IsInstantBooking = c.Boolean(nullable: false),
                        SurfaceSquareMeters = c.Int(nullable: false),
                        PrivacyType = c.String(),
                        AccommodType = c.String(),
                        SourceID = c.String(),
                        SourceSlug = c.String(),
                        SourceName = c.String(),
                        SearchRequestResults_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SearchRequestResults", t => t.SearchRequestResults_ID)
                .Index(t => t.SearchRequestResults_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserType = c.String(),
                        ProfileID = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SearchVMs", "Search_ID", "dbo.Searches");
            DropForeignKey("dbo.SearchVMs", "LikibuResults_ID", "dbo.SearchRequestResults");
            DropForeignKey("dbo.Offers", "SearchRequestResults_ID", "dbo.SearchRequestResults");
            DropForeignKey("dbo.SearchVMs", "FilterProps_ID", "dbo.FilterProperties");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Requests", "RequestTraveler_ID", "dbo.RoomRequests");
            DropForeignKey("dbo.RoomRequests", "Request_ID1", "dbo.Requests");
            DropForeignKey("dbo.Requests", "AvailableRooms_ID", "dbo.RoomsAvailables");
            DropForeignKey("dbo.BedRooms", "RoomsAvailable_ID", "dbo.RoomsAvailables");
            DropForeignKey("dbo.RoomRequests", "Request_ID", "dbo.Requests");
            DropForeignKey("dbo.RoomRequests", "RequestingTraveler_ID", "dbo.TravelerProfiles");
            DropForeignKey("dbo.Reviews", "Poster_ID", "dbo.TravelerProfiles");
            DropForeignKey("dbo.TravelerProfiles", "ReservedVillasID", "dbo.VillaReservations");
            DropForeignKey("dbo.Reservations", "VillaReservations_ID", "dbo.VillaReservations");
            DropForeignKey("dbo.TravelerProfiles", "Reservation_ID", "dbo.Reservations");
            DropForeignKey("dbo.Reservations", "ReservingTraveler_ID", "dbo.TravelerProfiles");
            DropForeignKey("dbo.Reservations", "ReservationTime_ID", "dbo.VillaReservationTimes");
            DropForeignKey("dbo.TravelerProfiles", "VillaRequestsID", "dbo.VillaRequests");
            DropForeignKey("dbo.Requests", "VillaRequests_ID", "dbo.VillaRequests");
            DropForeignKey("dbo.TravelerProfiles", "TravelerMailboxID", "dbo.Mailboxes");
            DropForeignKey("dbo.TravelerProfiles", "TravelerGalleryID", "dbo.Galleries");
            DropForeignKey("dbo.TravelerProfiles", "TravelerProfileDescriptionID", "dbo.TravelerProfileDescriptions");
            DropForeignKey("dbo.TravelerProfiles", "TravelerAttributesID", "dbo.TravelerProfileAttributes");
            DropForeignKey("dbo.RoomRequests", "RequestedTime_ID", "dbo.RequestTimes");
            DropForeignKey("dbo.BedRooms", "RoomRequest_ID", "dbo.RoomRequests");
            DropForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccomProfiles");
            DropForeignKey("dbo.Mails", "Sender_ID", "dbo.Mailboxes");
            DropForeignKey("dbo.Mailboxes", "Mail_ID", "dbo.Mails");
            DropForeignKey("dbo.Mails", "Mailbox_ID3", "dbo.Mailboxes");
            DropForeignKey("dbo.Mails", "Mailbox_ID2", "dbo.Mailboxes");
            DropForeignKey("dbo.Mails", "Mailbox_ID1", "dbo.Mailboxes");
            DropForeignKey("dbo.Mails", "Mailbox_ID", "dbo.Mailboxes");
            DropForeignKey("dbo.Attachements", "Mail_ID", "dbo.Mails");
            DropForeignKey("dbo.Galleries", "ProfilePic_ID", "dbo.Pictures");
            DropForeignKey("dbo.Pictures", "Gallery_ID", "dbo.Galleries");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Offers", new[] { "SearchRequestResults_ID" });
            DropIndex("dbo.SearchVMs", new[] { "Search_ID" });
            DropIndex("dbo.SearchVMs", new[] { "LikibuResults_ID" });
            DropIndex("dbo.SearchVMs", new[] { "FilterProps_ID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reviews", new[] { "Poster_ID" });
            DropIndex("dbo.Reservations", new[] { "VillaReservations_ID" });
            DropIndex("dbo.Reservations", new[] { "ReservingTraveler_ID" });
            DropIndex("dbo.Reservations", new[] { "ReservationTime_ID" });
            DropIndex("dbo.TravelerProfiles", new[] { "Reservation_ID" });
            DropIndex("dbo.TravelerProfiles", new[] { "ReservedVillasID" });
            DropIndex("dbo.TravelerProfiles", new[] { "VillaRequestsID" });
            DropIndex("dbo.TravelerProfiles", new[] { "TravelerGalleryID" });
            DropIndex("dbo.TravelerProfiles", new[] { "TravelerMailboxID" });
            DropIndex("dbo.TravelerProfiles", new[] { "TravelerAttributesID" });
            DropIndex("dbo.TravelerProfiles", new[] { "TravelerProfileDescriptionID" });
            DropIndex("dbo.BedRooms", new[] { "RoomsAvailable_ID" });
            DropIndex("dbo.BedRooms", new[] { "RoomRequest_ID" });
            DropIndex("dbo.RoomRequests", new[] { "Request_ID1" });
            DropIndex("dbo.RoomRequests", new[] { "Request_ID" });
            DropIndex("dbo.RoomRequests", new[] { "RequestingTraveler_ID" });
            DropIndex("dbo.RoomRequests", new[] { "RequestedTime_ID" });
            DropIndex("dbo.Requests", new[] { "RequestTraveler_ID" });
            DropIndex("dbo.Requests", new[] { "AvailableRooms_ID" });
            DropIndex("dbo.Requests", new[] { "VillaRequests_ID" });
            DropIndex("dbo.Requests", new[] { "AccomProfile_ID" });
            DropIndex("dbo.Mailboxes", new[] { "Mail_ID" });
            DropIndex("dbo.Mails", new[] { "Sender_ID" });
            DropIndex("dbo.Mails", new[] { "Mailbox_ID3" });
            DropIndex("dbo.Mails", new[] { "Mailbox_ID2" });
            DropIndex("dbo.Mails", new[] { "Mailbox_ID1" });
            DropIndex("dbo.Mails", new[] { "Mailbox_ID" });
            DropIndex("dbo.Pictures", new[] { "Gallery_ID" });
            DropIndex("dbo.Galleries", new[] { "ProfilePic_ID" });
            DropIndex("dbo.Attachements", new[] { "Mail_ID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Offers");
            DropTable("dbo.SearchRequestResults");
            DropTable("dbo.FilterProperties");
            DropTable("dbo.SearchVMs");
            DropTable("dbo.Searches");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RoomsAvailables");
            DropTable("dbo.Reviews");
            DropTable("dbo.VillaReservationTimes");
            DropTable("dbo.Reservations");
            DropTable("dbo.VillaReservations");
            DropTable("dbo.VillaRequests");
            DropTable("dbo.TravelerProfileDescriptions");
            DropTable("dbo.TravelerProfileAttributes");
            DropTable("dbo.TravelerProfiles");
            DropTable("dbo.RequestTimes");
            DropTable("dbo.BedRooms");
            DropTable("dbo.RoomRequests");
            DropTable("dbo.AccomProfiles");
            DropTable("dbo.Requests");
            DropTable("dbo.Mailboxes");
            DropTable("dbo.Mails");
            DropTable("dbo.SearchSessions");
            DropTable("dbo.Pictures");
            DropTable("dbo.Galleries");
            DropTable("dbo.Attachements");
        }
    }
}
