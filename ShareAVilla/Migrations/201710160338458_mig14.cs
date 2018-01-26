namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "Accommodation_ID", "dbo.Accommodations");
            DropForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccommodationProfiles");
            DropIndex("dbo.Requests", new[] { "AccomProfile_ID" });
            DropIndex("dbo.Requests", new[] { "Accommodation_ID" });
            AddColumn("dbo.Requests", "Accommodation_ID1", c => c.Int());
            AddColumn("dbo.Requests", "AccomProfile_ID1", c => c.Int());
            CreateIndex("dbo.Requests", "Accommodation_ID1");
            CreateIndex("dbo.Requests", "AccomProfile_ID1");
            AddForeignKey("dbo.Requests", "Accommodation_ID1", "dbo.Accommodations", "ID");
            AddForeignKey("dbo.Requests", "AccomProfile_ID1", "dbo.AccommodationProfiles", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "AccomProfile_ID1", "dbo.AccommodationProfiles");
            DropForeignKey("dbo.Requests", "Accommodation_ID1", "dbo.Accommodations");
            DropIndex("dbo.Requests", new[] { "AccomProfile_ID1" });
            DropIndex("dbo.Requests", new[] { "Accommodation_ID1" });
            DropColumn("dbo.Requests", "AccomProfile_ID1");
            DropColumn("dbo.Requests", "Accommodation_ID1");
            CreateIndex("dbo.Requests", "Accommodation_ID");
            CreateIndex("dbo.Requests", "AccomProfile_ID");
            AddForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccommodationProfiles", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "Accommodation_ID", "dbo.Accommodations", "ID", cascadeDelete: true);
        }
    }
}
