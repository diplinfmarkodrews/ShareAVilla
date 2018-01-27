namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "PriceMax", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "PriceMin", c => c.Single(nullable: false));
            AddColumn("dbo.RequestVMOwners", "MaxPrice", c => c.Single(nullable: false));
            AddColumn("dbo.RequestVMOwners", "MinPrice", c => c.Single(nullable: false));
            AddColumn("dbo.RequestVMOwners", "requestType", c => c.Int(nullable: false));
            DropColumn("dbo.Capacities", "Accom_ID");
            DropColumn("dbo.Requests", "PricePWeek");
            DropColumn("dbo.Requests", "PricePDay");
            DropColumn("dbo.Requests", "PricePMonth");
            DropColumn("dbo.RequestVMOwners", "thumbTemp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestVMOwners", "thumbTemp", c => c.String());
            AddColumn("dbo.Requests", "PricePMonth", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "PricePDay", c => c.Single(nullable: false));
            AddColumn("dbo.Requests", "PricePWeek", c => c.Single(nullable: false));
            AddColumn("dbo.Capacities", "Accom_ID", c => c.Int(nullable: false));
            DropColumn("dbo.RequestVMOwners", "requestType");
            DropColumn("dbo.RequestVMOwners", "MinPrice");
            DropColumn("dbo.RequestVMOwners", "MaxPrice");
            DropColumn("dbo.Requests", "PriceMin");
            DropColumn("dbo.Requests", "PriceMax");
        }
    }
}
