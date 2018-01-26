namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccommodationProfiles", "Thumbnails", c => c.String());
            AddColumn("dbo.AccommodationProfiles", "Photos", c => c.String());
            DropColumn("dbo.Requests", "BedRooms");
            DropColumn("dbo.Requests", "MaxGuests");
            DropColumn("dbo.Requests", "Destination");
            DropColumn("dbo.Requests", "Lat");
            DropColumn("dbo.Requests", "Lng");
            DropColumn("dbo.Requests", "Thumb");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "Thumb", c => c.String());
            AddColumn("dbo.Requests", "Lng", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "Lat", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "Destination", c => c.String());
            AddColumn("dbo.Requests", "MaxGuests", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "BedRooms", c => c.Int(nullable: false));
            DropColumn("dbo.AccommodationProfiles", "Photos");
            DropColumn("dbo.AccommodationProfiles", "Thumbnails");
        }
    }
}
