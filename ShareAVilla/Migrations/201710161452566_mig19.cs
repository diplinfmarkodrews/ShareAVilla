namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequestResponses", "objState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequestResponses", "objState");
        }
    }
}
