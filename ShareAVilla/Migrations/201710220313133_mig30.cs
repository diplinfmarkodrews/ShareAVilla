namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequests", "Nevermind", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequests", "Nevermind");
        }
    }
}
