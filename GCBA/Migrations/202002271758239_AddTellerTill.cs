namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTellerTill : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TellersTills", newName: "TellerTills");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TellerTills", newName: "TellersTills");
        }
    }
}
