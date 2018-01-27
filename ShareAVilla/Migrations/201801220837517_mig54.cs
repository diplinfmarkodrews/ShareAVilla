namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig54 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "TimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "TimeStamp");
        }
    }
}
