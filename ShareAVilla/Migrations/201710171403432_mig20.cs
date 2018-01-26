namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequests", "DoubleBedPropose", c => c.Int(nullable: false));
            AddColumn("dbo.RoomRequests", "SingleBedPropose", c => c.Int(nullable: false));
            AddColumn("dbo.RoomRequests", "ShareRoomPropose", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequests", "ShareRoomPropose");
            DropColumn("dbo.RoomRequests", "SingleBedPropose");
            DropColumn("dbo.RoomRequests", "DoubleBedPropose");
        }
    }
}
