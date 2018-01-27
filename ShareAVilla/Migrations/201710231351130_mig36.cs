namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequestVMOwners", "user", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequestVMOwners", "user");
        }
    }
}
