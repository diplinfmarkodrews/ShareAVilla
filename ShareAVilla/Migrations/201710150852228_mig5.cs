namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AccommodationProfiles", "Offer_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccommodationProfiles", "Offer_ID", c => c.Int(nullable: false));
        }
    }
}
