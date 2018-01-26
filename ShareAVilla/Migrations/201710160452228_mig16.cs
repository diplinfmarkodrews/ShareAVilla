namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccommodationProfiles", "LikibuOffer_ID", "dbo.Offers");
            DropIndex("dbo.AccommodationProfiles", new[] { "LikibuOffer_ID" });
            AddColumn("dbo.AccommodationProfiles", "Title", c => c.String());
            AddColumn("dbo.AccommodationProfiles", "Text", c => c.String());
            AddColumn("dbo.AccommodationProfiles", "BathRooms", c => c.Int(nullable: false));
            DropColumn("dbo.AccommodationProfiles", "LikibuOffer_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccommodationProfiles", "LikibuOffer_ID", c => c.Int(nullable: false));
            DropColumn("dbo.AccommodationProfiles", "BathRooms");
            DropColumn("dbo.AccommodationProfiles", "Text");
            DropColumn("dbo.AccommodationProfiles", "Title");
            CreateIndex("dbo.AccommodationProfiles", "LikibuOffer_ID");
            AddForeignKey("dbo.AccommodationProfiles", "LikibuOffer_ID", "dbo.Offers", "ID", cascadeDelete: true);
        }
    }
}
