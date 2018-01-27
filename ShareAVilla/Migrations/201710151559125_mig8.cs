namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestVMOwners", "thumbTemp", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestVMOwners", "thumbTemp");
        }
    }
}
