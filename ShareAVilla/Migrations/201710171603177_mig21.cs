namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccommodationProfiles");
            DropIndex("dbo.Requests", new[] { "AccomProfile_ID" });
            AddColumn("dbo.Accommodations", "AccomProfileID", c => c.Int(nullable: false));
            CreateIndex("dbo.Accommodations", "AccomProfileID");
            AddForeignKey("dbo.Accommodations", "AccomProfileID", "dbo.AccommodationProfiles", "ID", cascadeDelete: true);
            DropColumn("dbo.Requests", "AccomProfile_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "AccomProfile_ID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Accommodations", "AccomProfileID", "dbo.AccommodationProfiles");
            DropIndex("dbo.Accommodations", new[] { "AccomProfileID" });
            DropColumn("dbo.Accommodations", "AccomProfileID");
            CreateIndex("dbo.Requests", "AccomProfile_ID");
            AddForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccommodationProfiles", "ID", cascadeDelete: true);
        }
    }
}
