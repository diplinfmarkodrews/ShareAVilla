namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequestVMUsers", "CheckInOwner", c => c.String());
            AddColumn("dbo.RoomRequestVMUsers", "CheckOutOwner", c => c.String());
            AddColumn("dbo.RoomRequestVMUsers", "PriceOwner", c => c.Single(nullable: false));
            AddColumn("dbo.RoomRequestVMUsers", "ShareRoomOwner", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoomRequestVMUsers", "DoubleBedOwner", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoomRequestVMUsers", "SingleBedOwner", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoomRequestVMUsers", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.RoomRequestVMUsers", "AvailableRooms");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RoomRequestVMUsers", "AvailableRooms", c => c.Int(nullable: false));
            DropColumn("dbo.RoomRequestVMUsers", "Status");
            DropColumn("dbo.RoomRequestVMUsers", "SingleBedOwner");
            DropColumn("dbo.RoomRequestVMUsers", "DoubleBedOwner");
            DropColumn("dbo.RoomRequestVMUsers", "ShareRoomOwner");
            DropColumn("dbo.RoomRequestVMUsers", "PriceOwner");
            DropColumn("dbo.RoomRequestVMUsers", "CheckOutOwner");
            DropColumn("dbo.RoomRequestVMUsers", "CheckInOwner");
        }
    }
}
