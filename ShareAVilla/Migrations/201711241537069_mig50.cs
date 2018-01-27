namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig50 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestVMListings", "SearchVM_ID", "dbo.SearchVMs");
            DropIndex("dbo.RequestVMListings", new[] { "SearchVM_ID" });
            CreateTable(
                "dbo.RoomRequestVMOwners",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicantName = c.String(),
                        DoubleBed = c.Boolean(nullable: false),
                        SingleBed = c.Boolean(nullable: false),
                        ShareRoom = c.Boolean(nullable: false),
                        CheckIn = c.String(),
                        CheckOut = c.String(),
                        Price = c.String(),
                        Message = c.String(),
                        RoomID = c.Int(nullable: false),
                        user = c.Boolean(nullable: false),
                        Request_ID = c.Int(nullable: false),
                        RoomType = c.String(),
                        ApplyingTraveler_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProfilePreviewVMs", t => t.ApplyingTraveler_ID)
                .Index(t => t.ApplyingTraveler_ID);
            
            CreateTable(
                "dbo.RequestResults",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TotalResults = c.Int(nullable: false),
                        Page = c.Int(nullable: false),
                        PerPage = c.Int(nullable: false),
                        objState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Capacities", "NumBedRooms", c => c.Int(nullable: false));
            AddColumn("dbo.RoomRequestVMUsers", "Host_ID", c => c.Int());
            AddColumn("dbo.Searches", "nav", c => c.String());
            AddColumn("dbo.SearchVMs", "RequestResults_ID", c => c.Int());
            AddColumn("dbo.RequestVMListings", "Host_ID", c => c.Int());
            AddColumn("dbo.RequestVMListings", "RequestResults_ID", c => c.Int());
            AddColumn("dbo.TravelerProfileAttributes", "Yoga", c => c.Boolean(nullable: false));
            CreateIndex("dbo.RequestVMListings", "Host_ID");
            CreateIndex("dbo.RequestVMListings", "RequestResults_ID");
            CreateIndex("dbo.RoomRequestVMUsers", "Host_ID");
            CreateIndex("dbo.SearchVMs", "RequestResults_ID");
            AddForeignKey("dbo.RequestVMListings", "Host_ID", "dbo.ProfilePreviewVMs", "ID");
            AddForeignKey("dbo.RoomRequestVMUsers", "Host_ID", "dbo.ProfilePreviewVMs", "ID");
            AddForeignKey("dbo.RequestVMListings", "RequestResults_ID", "dbo.RequestResults", "ID");
            AddForeignKey("dbo.SearchVMs", "RequestResults_ID", "dbo.RequestResults", "ID");
            DropColumn("dbo.Capacities", "BedRooms");
            DropColumn("dbo.RequestVMListings", "SearchVM_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestVMListings", "SearchVM_ID", c => c.Int());
            AddColumn("dbo.Capacities", "BedRooms", c => c.Int(nullable: false));
            DropForeignKey("dbo.SearchVMs", "RequestResults_ID", "dbo.RequestResults");
            DropForeignKey("dbo.RequestVMListings", "RequestResults_ID", "dbo.RequestResults");
            DropForeignKey("dbo.RoomRequestVMUsers", "Host_ID", "dbo.ProfilePreviewVMs");
            DropForeignKey("dbo.RoomRequestVMOwners", "ApplyingTraveler_ID", "dbo.ProfilePreviewVMs");
            DropForeignKey("dbo.RequestVMListings", "Host_ID", "dbo.ProfilePreviewVMs");
            DropIndex("dbo.SearchVMs", new[] { "RequestResults_ID" });
            DropIndex("dbo.RoomRequestVMUsers", new[] { "Host_ID" });
            DropIndex("dbo.RoomRequestVMOwners", new[] { "ApplyingTraveler_ID" });
            DropIndex("dbo.RequestVMListings", new[] { "RequestResults_ID" });
            DropIndex("dbo.RequestVMListings", new[] { "Host_ID" });
            DropColumn("dbo.TravelerProfileAttributes", "Yoga");
            DropColumn("dbo.RequestVMListings", "RequestResults_ID");
            DropColumn("dbo.RequestVMListings", "Host_ID");
            DropColumn("dbo.SearchVMs", "RequestResults_ID");
            DropColumn("dbo.Searches", "nav");
            DropColumn("dbo.RoomRequestVMUsers", "Host_ID");
            DropColumn("dbo.Capacities", "NumBedRooms");
            DropTable("dbo.RequestResults");
            DropTable("dbo.RoomRequestVMOwners");
            CreateIndex("dbo.RequestVMListings", "SearchVM_ID");
            AddForeignKey("dbo.RequestVMListings", "SearchVM_ID", "dbo.SearchVMs", "ID");
        }
    }
}
