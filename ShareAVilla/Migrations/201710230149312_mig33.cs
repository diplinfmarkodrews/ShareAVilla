namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BedRoomVMs", "Shared", c => c.Boolean(nullable: false));
            DropColumn("dbo.BedRooms", "Accom_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BedRooms", "Accom_ID", c => c.Int(nullable: false));
            DropColumn("dbo.BedRoomVMs", "Shared");
        }
    }
}
