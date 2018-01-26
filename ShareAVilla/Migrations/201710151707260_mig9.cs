namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelerProfileSearchSessions", "Filter_ID", c => c.Int());
            CreateIndex("dbo.TravelerProfileSearchSessions", "Filter_ID");
            AddForeignKey("dbo.TravelerProfileSearchSessions", "Filter_ID", "dbo.FilterProperties", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelerProfileSearchSessions", "Filter_ID", "dbo.FilterProperties");
            DropIndex("dbo.TravelerProfileSearchSessions", new[] { "Filter_ID" });
            DropColumn("dbo.TravelerProfileSearchSessions", "Filter_ID");
        }
    }
}
