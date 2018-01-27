namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequests", "RequestOwner", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequests", "RequestOwner");
        }
    }
}
