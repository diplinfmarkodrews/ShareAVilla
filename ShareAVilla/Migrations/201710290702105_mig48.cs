namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig48 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomRequestVMUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CheckInRequest = c.String(),
                        CheckOutRequest = c.String(),
                        CheckInOwner = c.String(),
                        CheckOutOwner = c.String(),
                        CheckIn = c.String(nullable: false),
                        CheckOut = c.String(nullable: false),
                        Price = c.String(nullable: false),
                        PriceOwner = c.String(),
                        Text = c.String(),
                        PricePNigthPRoom = c.String(),
                        PricePWeekPRoom = c.String(),
                        PricePMonthPRoom = c.String(),
                        PricePRoomPTime = c.String(),
                        PriceSpan = c.String(),
                        BedRooms = c.Int(nullable: false),
                        ShareRoom = c.Boolean(nullable: false),
                        DoubleBed = c.Boolean(nullable: false),
                        SingleBed = c.Boolean(nullable: false),
                        ShareRoomOwner = c.Boolean(nullable: false),
                        DoubleBedOwner = c.Boolean(nullable: false),
                        SingleBedOwner = c.Boolean(nullable: false),
                        Nevermind = c.Boolean(nullable: false),
                        RqType = c.Int(nullable: false),
                        RequestOwner = c.Boolean(nullable: false),
                        Request_ID = c.Int(nullable: false),
                        RoomRqID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RoomRequestVMUsers");
        }
    }
}
