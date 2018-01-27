namespace ShareAVilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig11 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Requests", name: "AccomProfileID", newName: "AccomProfile_ID");
            RenameIndex(table: "dbo.Requests", name: "IX_AccomProfileID", newName: "IX_AccomProfile_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Requests", name: "IX_AccomProfile_ID", newName: "IX_AccomProfileID");
            RenameColumn(table: "dbo.Requests", name: "AccomProfile_ID", newName: "AccomProfileID");
        }
    }
}
