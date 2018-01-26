namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccommodationProfiles", "LikibuOffer_ID", "dbo.Offers");
            DropIndex("dbo.AccommodationProfiles", new[] { "LikibuOffer_ID" });
            AlterColumn("dbo.AccommodationProfiles", "Likibuoffer_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.AccommodationProfiles", "Likibuoffer_ID");
            AddForeignKey("dbo.AccommodationProfiles", "Likibuoffer_ID", "dbo.Offers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccommodationProfiles", "Likibuoffer_ID", "dbo.Offers");
            DropIndex("dbo.AccommodationProfiles", new[] { "Likibuoffer_ID" });
            AlterColumn("dbo.AccommodationProfiles", "Likibuoffer_ID", c => c.Int());
            CreateIndex("dbo.AccommodationProfiles", "LikibuOffer_ID");
            AddForeignKey("dbo.AccommodationProfiles", "LikibuOffer_ID", "dbo.Offers", "ID");
        }
    }
}
