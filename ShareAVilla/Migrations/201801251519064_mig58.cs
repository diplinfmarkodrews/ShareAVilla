namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig58 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RoomRequestVMOwners", "CheckIn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomRequestVMOwners", "CheckOut", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RoomRequestVMOwners", "CheckOut", c => c.String());
            AlterColumn("dbo.RoomRequestVMOwners", "CheckIn", c => c.String());
        }
    }
}
