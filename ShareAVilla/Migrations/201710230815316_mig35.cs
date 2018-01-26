namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomRequestVMOwners", "ApplicantName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequestVMOwners", "ApplicantName");
        }
    }
}
