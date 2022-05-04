namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PutConsumerWhereCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerAccounts", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.CustomerAccounts", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.CustomerAccounts", "LinkedAccountID", "dbo.CustomerAccounts");
            DropIndex("dbo.CustomerAccounts", new[] { "BranchID" });
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerID" });
            DropIndex("dbo.CustomerAccounts", new[] { "LinkedAccountID" });
            CreateTable(
                "dbo.ConsumerAccounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountName = c.String(nullable: false),
                        AccountNumber = c.String(),
                        AccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchID = c.Int(nullable: false),
                        AccountStatus = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                        ConsumerID = c.Int(nullable: false),
                        LoanMonthlyInterestRepay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanMonthlyRepay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanMonthlyPrincipalRepay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanPrincipalRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TermsOfLoan = c.Int(),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkedAccountID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.BranchID, cascadeDelete: true)
                .ForeignKey("dbo.Consumers", t => t.ConsumerID, cascadeDelete: true)
                .ForeignKey("dbo.ConsumerAccounts", t => t.LinkedAccountID)
                .Index(t => t.BranchID)
                .Index(t => t.ConsumerID)
                .Index(t => t.LinkedAccountID);
            
            CreateTable(
                "dbo.Consumers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ConsumerLongID = c.String(),
                        FullName = c.String(nullable: false, maxLength: 40),
                        Address = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 16),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.CustomerAccounts");
            DropTable("dbo.Customers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerLongId = c.String(),
                        FullName = c.String(nullable: false, maxLength: 40),
                        Address = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 16),
                        Gender = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CustomerAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountName = c.String(nullable: false),
                        AccountNumber = c.String(),
                        AccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchID = c.Int(nullable: false),
                        AccountStatus = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        LoanMonthlyInterestRepay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanMonthlyRepay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanMonthlyPrincipalRepay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanPrincipalRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TermsOfLoan = c.Int(),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkedAccountID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ConsumerAccounts", "LinkedAccountID", "dbo.ConsumerAccounts");
            DropForeignKey("dbo.ConsumerAccounts", "ConsumerID", "dbo.Consumers");
            DropForeignKey("dbo.ConsumerAccounts", "BranchID", "dbo.Branches");
            DropIndex("dbo.ConsumerAccounts", new[] { "LinkedAccountID" });
            DropIndex("dbo.ConsumerAccounts", new[] { "ConsumerID" });
            DropIndex("dbo.ConsumerAccounts", new[] { "BranchID" });
            DropTable("dbo.Consumers");
            DropTable("dbo.ConsumerAccounts");
            CreateIndex("dbo.CustomerAccounts", "LinkedAccountID");
            CreateIndex("dbo.CustomerAccounts", "CustomerID");
            CreateIndex("dbo.CustomerAccounts", "BranchID");
            AddForeignKey("dbo.CustomerAccounts", "LinkedAccountID", "dbo.CustomerAccounts", "Id");
            AddForeignKey("dbo.CustomerAccounts", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CustomerAccounts", "BranchID", "dbo.Branches", "ID", cascadeDelete: true);
        }
    }
}
