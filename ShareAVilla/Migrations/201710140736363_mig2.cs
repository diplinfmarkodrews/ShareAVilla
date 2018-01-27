namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccommodationProfiles", "Offer_ID", c => c.Int(nullable: false));
            DropColumn("dbo.AccommodationProfiles", "AccommodationProfile_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccommodationProfiles", "AccommodationProfile_ID", c => c.Int(nullable: false));
            DropColumn("dbo.AccommodationProfiles", "Offer_ID");
        }
    }
}
