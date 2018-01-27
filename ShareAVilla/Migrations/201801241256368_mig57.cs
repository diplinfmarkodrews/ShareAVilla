namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig57 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RoomRequestVMUsers", "CheckInOwner", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomRequestVMUsers", "CheckOutOwner", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RoomRequestVMUsers", "CheckOutOwner", c => c.String());
            AlterColumn("dbo.RoomRequestVMUsers", "CheckInOwner", c => c.String());
        }
    }
}
