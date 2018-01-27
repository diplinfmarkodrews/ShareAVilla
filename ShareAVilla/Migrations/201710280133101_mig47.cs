namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig47 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "ProfileReviewsID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "ProfileReviewsID");
        }
    }
}
