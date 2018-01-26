namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig13 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AccommodationProfiles", new[] { "Likibuoffer_ID" });
            CreateIndex("dbo.AccommodationProfiles", "LikibuOffer_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AccommodationProfiles", new[] { "LikibuOffer_ID" });
            CreateIndex("dbo.AccommodationProfiles", "Likibuoffer_ID");
        }
    }
}
