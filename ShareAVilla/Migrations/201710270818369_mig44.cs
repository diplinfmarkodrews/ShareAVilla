namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig44 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.AccommodationProfiles", "")

            AlterColumn("dbo.AccommodationProfiles", "PricePNight", c => c.String());
            AlterColumn("dbo.AccommodationProfiles", "PricePWeek", c => c.String());
            AlterColumn("dbo.AccommodationProfiles", "PricepMonth", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccommodationProfiles", "PricepMonth", c => c.Single(nullable: false));
            AlterColumn("dbo.AccommodationProfiles", "PricePWeek", c => c.Single(nullable: false));
            AlterColumn("dbo.AccommodationProfiles", "PricePNight", c => c.Single(nullable: false));
        }
    }
}
