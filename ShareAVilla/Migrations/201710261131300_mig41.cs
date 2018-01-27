namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig41 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RequestVMOwners");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestVMOwners",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CheckIn = c.String(),
                        CheckOut = c.String(),
                        Location = c.String(),
                        BedRooms = c.Int(nullable: false),
                        MaxGuests = c.Int(nullable: false),
                        PricePNight = c.Single(nullable: false),
                        PricePWeek = c.Single(nullable: false),
                        PricepMonth = c.Single(nullable: false),
                        SalesPricePNight = c.Single(nullable: false),
                        SalesPricePWeek = c.Single(nullable: false),
                        SalesPricepMonth = c.Single(nullable: false),
                        Title = c.String(),
                        Text = c.String(),
                        PricePRoomPerTime = c.Single(nullable: false),
                        TotalPricePRq = c.Single(nullable: false),
                        RoomRequests = c.String(),
                        MaxPrice = c.Single(nullable: false),
                        MinPrice = c.Single(nullable: false),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        AccomID = c.String(),
                        Link = c.String(),
                        requestType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
