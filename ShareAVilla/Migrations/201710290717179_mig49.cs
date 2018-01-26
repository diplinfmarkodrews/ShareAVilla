namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig49 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequestVMUsers", "Roomtype", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequestVMUsers", "Roomtype");
        }
    }
}
