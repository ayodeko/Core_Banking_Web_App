namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerIdChange : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerID" });
            CreateIndex("dbo.CustomerAccounts", "CustomerID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerID" });
            CreateIndex("dbo.CustomerAccounts", "CustomerID");
        }
    }
}
