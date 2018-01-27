namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "Accommodation_ID1", "dbo.Accommodations");
            DropForeignKey("dbo.Requests", "AccomProfile_ID1", "dbo.AccommodationProfiles");
            DropForeignKey("dbo.Requests", "Accommodation_ID1", "dbo.AvailableBedRooms");
            //DropTable("dbo.AvailableBedRooms");
            DropIndex("dbo.Requests", new[] { "Accommodation_ID1" });
            DropIndex("dbo.Requests", new[] { "AccomProfile_ID1" });
            DropColumn("dbo.Requests", "Accommodation_ID1");
            DropColumn("dbo.Requests", "AccomProfile_ID1");
            //RenameColumn(table: "dbo.Requests", name: "Accommodation_ID1", newName: "Accommodation_ID");
            //RenameColumn(table: "dbo.Requests", name: "AccomProfile_ID1", newName: "AccomProfile_ID");
            AlterColumn("dbo.Requests", "Accommodation_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "AccomProfile_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "Accommodation_ID");
            CreateIndex("dbo.Requests", "AccomProfile_ID");
            AddForeignKey("dbo.Requests", "Accommodation_ID", "dbo.Accommodations", "ID", cascadeDelete: false);
            AddForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccommodationProfiles", "ID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "AccomProfile_ID", "dbo.AccommodationProfiles");
            DropForeignKey("dbo.Requests", "Accommodation_ID", "dbo.Accommodations");
            DropIndex("dbo.Requests", new[] { "AccomProfile_ID" });
            DropIndex("dbo.Requests", new[] { "Accommodation_ID" });
            AlterColumn("dbo.Requests", "AccomProfile_ID", c => c.Int());
            AlterColumn("dbo.Requests", "Accommodation_ID", c => c.Int());
            RenameColumn(table: "dbo.Requests", name: "AccomProfile_ID", newName: "AccomProfile_ID1");
            RenameColumn(table: "dbo.Requests", name: "Accommodation_ID", newName: "Accommodation_ID1");
            AddColumn("dbo.Requests", "AccomProfile_ID", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "Accommodation_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "AccomProfile_ID1");
            CreateIndex("dbo.Requests", "Accommodation_ID1");
            AddForeignKey("dbo.Requests", "AccomProfile_ID1", "dbo.AccommodationProfiles", "ID");
            AddForeignKey("dbo.Requests", "Accommodation_ID1", "dbo.Accommodations", "ID");
        }
    }
}
