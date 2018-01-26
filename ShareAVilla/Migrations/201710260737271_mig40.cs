namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfilePreviewVMs", "Score", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfilePreviewVMs", "Score");
        }
    }
}
