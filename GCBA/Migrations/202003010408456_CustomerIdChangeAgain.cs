namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerIdChangeAgain : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerId" });
            CreateIndex("dbo.CustomerAccounts", "CustomerID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerID" });
            CreateIndex("dbo.CustomerAccounts", "CustomerId");
        }
    }
}
