namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountTypeManagement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountTypeManagements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CurrentCreditInterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentMinimumBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        COTIncomeID = c.Int(),
                        COT = c.Double(nullable: false),
                        CurrentInterestExpenseGlID = c.Int(),
                        SavingsCreditInterestRate = c.Double(nullable: false),
                        SavingsMinimumBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InterestExpenseGlID = c.Int(),
                        InterestPayableGlID = c.Int(),
                        LoanDebitInterestRate = c.Double(nullable: false),
                        InterestIncomeGlID = c.Int(),
                        InterestReceivableGlID = c.Int(),
                        IsOpened = c.Boolean(nullable: false),
                        FinancialDate = c.DateTime(nullable: false),
                        COTIncomeGl_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GlAccounts", t => t.COTIncomeGl_ID)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentInterestExpenseGlID)
                .ForeignKey("dbo.GlAccounts", t => t.InterestExpenseGlID)
                .ForeignKey("dbo.GlAccounts", t => t.InterestIncomeGlID)
                .ForeignKey("dbo.GlAccounts", t => t.InterestPayableGlID)
                .ForeignKey("dbo.GlAccounts", t => t.InterestReceivableGlID)
                .Index(t => t.CurrentInterestExpenseGlID)
                .Index(t => t.InterestExpenseGlID)
                .Index(t => t.InterestPayableGlID)
                .Index(t => t.InterestIncomeGlID)
                .Index(t => t.InterestReceivableGlID)
                .Index(t => t.COTIncomeGl_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountTypeManagements", "InterestReceivableGlID", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountTypeManagements", "InterestPayableGlID", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountTypeManagements", "InterestIncomeGlID", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountTypeManagements", "InterestExpenseGlID", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountTypeManagements", "CurrentInterestExpenseGlID", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountTypeManagements", "COTIncomeGl_ID", "dbo.GlAccounts");
            DropIndex("dbo.AccountTypeManagements", new[] { "COTIncomeGl_ID" });
            DropIndex("dbo.AccountTypeManagements", new[] { "InterestReceivableGlID" });
            DropIndex("dbo.AccountTypeManagements", new[] { "InterestIncomeGlID" });
            DropIndex("dbo.AccountTypeManagements", new[] { "InterestPayableGlID" });
            DropIndex("dbo.AccountTypeManagements", new[] { "InterestExpenseGlID" });
            DropIndex("dbo.AccountTypeManagements", new[] { "CurrentInterestExpenseGlID" });
            DropTable("dbo.AccountTypeManagements");
        }
    }
}
