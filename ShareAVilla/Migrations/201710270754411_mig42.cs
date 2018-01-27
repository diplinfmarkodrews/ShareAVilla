namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig42 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoomRequestVMOwners", "ApplyingTraveler_ID", "dbo.ProfilePreviewVMs");
            DropIndex("dbo.RoomRequestVMOwners", new[] { "ApplyingTraveler_ID" });
            DropColumn("dbo.Requests", "PriceMax");
            DropColumn("dbo.Requests", "PriceMin");
            DropColumn("dbo.Requests", "SalesPricePWeek");
            DropColumn("dbo.Requests", "SalesPricePDay");
            DropColumn("dbo.Requests", "SalesPricePMonth");
            DropColumn("dbo.RoomRequests", "PriceUser");
            DropColumn("dbo.RoomRequests", "PriceOwner");
            DropTable("dbo.RoomRequestVMOwners");
            DropTable("dbo.RoomRequestVMUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoomRequestVMUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CheckInRequest = c.String(),
                        CheckOutRequest = c.String(),
                        CheckInOwner = c.String(),
                        CheckOutOwner = c.String(),
                        CheckIn = c.String(nullable: false),
                        CheckOut = c.String(nullable: false),
                        Price = c.Single(nullable: false),
                        PriceOwner = c.Single(nullable: false),
                        Text = c.String(),
                        PricePNigthPRoom = c.Single(nullable: false),
                        PricePWeekPRoom = c.Single(nullable: false),
                        PricePMonthPRoom = c.Single(nullable: false),
                        PricePRoomPTime = c.Single(nullable: false),
                        BedRooms = c.Int(nullable: false),
                        ShareRoom = c.Boolean(nullable: false),
                        DoubleBed = c.Boolean(nullable: false),
                        SingleBed = c.Boolean(nullable: false),
                        ShareRoomOwner = c.Boolean(nullable: false),
                        DoubleBedOwner = c.Boolean(nullable: false),
                        SingleBedOwner = c.Boolean(nullable: false),
                        Nevermind = c.Boolean(nullable: false),
                        RequestOwner = c.Boolean(nullable: false),
                        Request_ID = c.Int(nullable: false),
                        RoomRqID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        Price = c.Single(nullable: false),
                        Message = c.String(),
                        RoomID = c.Int(nullable: false),
                        user = c.Boolean(nullable: false),
                        Request_ID = c.Int(nullable: false),
                        ApplyingTraveler_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.RoomRequests", "PriceOwner", c => c.Single(nullable: false));
            AddColumn("dbo.RoomRequests", "PriceUser", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "SalesPricePMonth", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "SalesPricePDay", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "SalesPricePWeek", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "PriceMin", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "PriceMax", c => c.Single(nullable: false));
            CreateIndex("dbo.RoomRequestVMOwners", "ApplyingTraveler_ID");
            AddForeignKey("dbo.RoomRequestVMOwners", "ApplyingTraveler_ID", "dbo.ProfilePreviewVMs", "ID");
        }
    }
}
