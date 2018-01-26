namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "PriceMax", c => c.String());
            AddColumn("dbo.Requests", "PriceMin", c => c.String());
            AddColumn("dbo.Requests", "SalesPricePWeek", c => c.String());
            AddColumn("dbo.Requests", "SalesPricePDay", c => c.String());
            AddColumn("dbo.Requests", "SalesPricePMonth", c => c.String());
            AddColumn("dbo.RoomRequests", "PriceUser", c => c.String());
            AddColumn("dbo.RoomRequests", "PriceOwner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomRequests", "PriceOwner");
            DropColumn("dbo.RoomRequests", "PriceUser");
            DropColumn("dbo.Requests", "SalesPricePMonth");
            DropColumn("dbo.Requests", "SalesPricePDay");
            DropColumn("dbo.Requests", "SalesPricePWeek");
            DropColumn("dbo.Requests", "PriceMin");
            DropColumn("dbo.Requests", "PriceMax");
        }
    }
}
