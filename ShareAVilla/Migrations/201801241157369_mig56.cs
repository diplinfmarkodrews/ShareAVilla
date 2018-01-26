namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig56 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RoomRequestVMUsers", "CheckInRequest", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomRequestVMUsers", "CheckOutRequest", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomRequestVMUsers", "CheckIn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomRequestVMUsers", "CheckOut", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RoomRequestVMUsers", "CheckOut", c => c.String(nullable: false));
            AlterColumn("dbo.RoomRequestVMUsers", "CheckIn", c => c.String(nullable: false));
            AlterColumn("dbo.RoomRequestVMUsers", "CheckOutRequest", c => c.String());
            AlterColumn("dbo.RoomRequestVMUsers", "CheckInRequest", c => c.String());
        }
    }
}
