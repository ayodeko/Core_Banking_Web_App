namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientAccounts",
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
                        DaysCount = c.Int(),
                        dailyInterestAccrued = c.Decimal(precision: 18, scale: 2),
                        LoanInterestRatePerMonth = c.Decimal(precision: 18, scale: 2),
                        SavingsWithdrawalCount = c.Int(),
                        CurrentLien = c.Decimal(precision: 18, scale: 2),
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
                .ForeignKey("dbo.ClientAccounts", t => t.LinkedAccountID)
                .Index(t => t.BranchID)
                .Index(t => t.ConsumerID)
                .Index(t => t.LinkedAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientAccounts", "LinkedAccountID", "dbo.ClientAccounts");
            DropForeignKey("dbo.ClientAccounts", "ConsumerID", "dbo.Consumers");
            DropForeignKey("dbo.ClientAccounts", "BranchID", "dbo.Branches");
            DropIndex("dbo.ClientAccounts", new[] { "LinkedAccountID" });
            DropIndex("dbo.ClientAccounts", new[] { "ConsumerID" });
            DropIndex("dbo.ClientAccounts", new[] { "BranchID" });
            DropTable("dbo.ClientAccounts");
        }
    }
}
