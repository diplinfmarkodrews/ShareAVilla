namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccommodationProfiles", "DestinationIDs", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccommodationProfiles", "DestinationIDs");
        }
    }
}
