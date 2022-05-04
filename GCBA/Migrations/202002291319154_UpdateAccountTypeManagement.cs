namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountTypeManagement : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AccountTypeManagements", name: "COTIncomeGl_ID", newName: "COTIncomeGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "InterestExpenseGlID", newName: "LoanInterestIncomeGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "InterestIncomeGlID", newName: "LoanInterestReceivableGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "InterestPayableGlID", newName: "SavingsInterestExpenseGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "InterestReceivableGlID", newName: "SavingsInterestPayableGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_COTIncomeGl_ID", newName: "IX_COTIncomeGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_InterestPayableGlID", newName: "IX_SavingsInterestExpenseGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_InterestReceivableGlID", newName: "IX_SavingsInterestPayableGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_InterestExpenseGlID", newName: "IX_LoanInterestIncomeGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_InterestIncomeGlID", newName: "IX_LoanInterestReceivableGlID");
            AlterColumn("dbo.AccountTypeManagements", "CurrentCreditInterestRate", c => c.Double(nullable: false));
            AlterColumn("dbo.AccountTypeManagements", "COT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AccountTypeManagements", "COTIncomeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountTypeManagements", "COTIncomeID", c => c.Int());
            AlterColumn("dbo.AccountTypeManagements", "COT", c => c.Double(nullable: false));
            AlterColumn("dbo.AccountTypeManagements", "CurrentCreditInterestRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_LoanInterestReceivableGlID", newName: "IX_InterestIncomeGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_LoanInterestIncomeGlID", newName: "IX_InterestExpenseGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_SavingsInterestPayableGlID", newName: "IX_InterestReceivableGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_SavingsInterestExpenseGlID", newName: "IX_InterestPayableGlID");
            RenameIndex(table: "dbo.AccountTypeManagements", name: "IX_COTIncomeGlID", newName: "IX_COTIncomeGl_ID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "SavingsInterestPayableGlID", newName: "InterestReceivableGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "SavingsInterestExpenseGlID", newName: "InterestPayableGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "LoanInterestReceivableGlID", newName: "InterestIncomeGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "LoanInterestIncomeGlID", newName: "InterestExpenseGlID");
            RenameColumn(table: "dbo.AccountTypeManagements", name: "COTIncomeGlID", newName: "COTIncomeGl_ID");
        }
    }
}
