namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig17 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "AccomProfileID", "dbo.Offers");
        }
        
        public override void Down()
        {
        }
    }
}
