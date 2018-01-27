namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Type");
        }
    }
}
