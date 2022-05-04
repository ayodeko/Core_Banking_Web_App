namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerAccount1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountName = c.String(nullable: false),
                        AccountNumber = c.String(),
                        AccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        AccountStatus = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchID, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.BranchID)
                .Index(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerAccounts", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.CustomerAccounts", "BranchID", "dbo.Branches");
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerID" });
            DropIndex("dbo.CustomerAccounts", new[] { "BranchID" });
            DropTable("dbo.CustomerAccounts");
        }
    }
}
