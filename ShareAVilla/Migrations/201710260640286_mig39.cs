namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig39 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TravelerProfileReviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Score = c.Single(nullable: false),
                        objState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Reviews", "TravelerProfileReviews_ID", c => c.Int());
            AddColumn("dbo.TravelerProfiles", "TravelerReviewsID", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "TravelerProfileReviews_ID");
            CreateIndex("dbo.TravelerProfiles", "TravelerReviewsID");
            AddForeignKey("dbo.Reviews", "TravelerProfileReviews_ID", "dbo.TravelerProfileReviews", "ID");
            AddForeignKey("dbo.TravelerProfiles", "TravelerReviewsID", "dbo.TravelerProfileReviews", "ID", cascadeDelete: true);
            DropColumn("dbo.ReviewVMs", "Ranking");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReviewVMs", "Ranking", c => c.Single(nullable: false));
            DropForeignKey("dbo.TravelerProfiles", "TravelerReviewsID", "dbo.TravelerProfileReviews");
            DropForeignKey("dbo.Reviews", "TravelerProfileReviews_ID", "dbo.TravelerProfileReviews");
            DropIndex("dbo.TravelerProfiles", new[] { "TravelerReviewsID" });
            DropIndex("dbo.Reviews", new[] { "TravelerProfileReviews_ID" });
            DropColumn("dbo.TravelerProfiles", "TravelerReviewsID");
            DropColumn("dbo.Reviews", "TravelerProfileReviews_ID");
            DropTable("dbo.TravelerProfileReviews");
        }
    }
}
