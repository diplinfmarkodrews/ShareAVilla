namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accommodations", "AccomProfile_ID", "dbo.AccommodationProfiles");
            DropIndex("dbo.Accommodations", new[] { "AccomProfile_ID" });
            DropColumn("dbo.Accommodations", "AccomProfile_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accommodations", "AccomProfile_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Accommodations", "AccomProfile_ID");
            AddForeignKey("dbo.Accommodations", "AccomProfile_ID", "dbo.AccommodationProfiles", "ID", cascadeDelete: true);
        }
    }
}
