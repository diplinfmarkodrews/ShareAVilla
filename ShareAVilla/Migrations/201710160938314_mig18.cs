namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccommodationProfiles", "AType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccommodationProfiles", "AType");
        }
    }
}
