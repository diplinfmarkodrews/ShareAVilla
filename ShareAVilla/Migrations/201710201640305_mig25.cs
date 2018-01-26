namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequestVMOwners", "Request_ID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequestVMOwners", "Request_ID");
        }
    }
}
