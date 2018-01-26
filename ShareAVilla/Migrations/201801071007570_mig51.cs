namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig51 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviews", "Ranking", c => c.Single(nullable: false));
            AlterColumn("dbo.ReviewVMs", "Points", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReviewVMs", "Points", c => c.Int(nullable: false));
            AlterColumn("dbo.Reviews", "Ranking", c => c.Int(nullable: false));
        }
    }
}
