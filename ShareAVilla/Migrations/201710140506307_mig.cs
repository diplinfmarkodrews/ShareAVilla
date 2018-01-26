namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccommodationProfiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        AccommodationProfile_ID = c.Int(nullable: false),
                        BedRooms = c.Int(nullable: false),
                        MaxGuests = c.Int(nullable: false),
                        AccomID = c.String(),
                        PricePNight = c.Single(nullable: false),
                        PricePWeek = c.Single(nullable: false),
                        PricepMonth = c.Single(nullable: false),
                        Location = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        URL = c.String(),
                        objState = c.Int(nullable: false),
                        LikibuOffer_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Offers", t => t.LikibuOffer_ID)
                .Index(t => t.LikibuOffer_ID);
            
            AddColumn("dbo.Accommodations", "AccomProfile_ID", c => c.Int(nullable: false));
            AddColumn("dbo.BedRooms", "Accom_ID", c => c.Int(nullable: false));
            AddColumn("dbo.Capacities", "Accom_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Accommodations", "AccomProfile_ID");
            AddForeignKey("dbo.Accommodations", "AccomProfile_ID", "dbo.AccommodationProfiles", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accommodations", "AccomProfile_ID", "dbo.AccommodationProfiles");
            DropForeignKey("dbo.AccommodationProfiles", "LikibuOffer_ID", "dbo.Offers");
            DropIndex("dbo.AccommodationProfiles", new[] { "LikibuOffer_ID" });
            DropIndex("dbo.Accommodations", new[] { "AccomProfile_ID" });
            DropColumn("dbo.Capacities", "Accom_ID");
            DropColumn("dbo.BedRooms", "Accom_ID");
            DropColumn("dbo.Accommodations", "AccomProfile_ID");
            DropTable("dbo.AccommodationProfiles");
        }
    }
}
