namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLoansToCustomerDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAccounts", "LoanMonthlyInterestRepay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CustomerAccounts", "LoanMonthlyRepay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CustomerAccounts", "LoanMonthlyPrincipalRepay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CustomerAccounts", "LoanPrincipalRemaining", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CustomerAccounts", "TermsOfLoan", c => c.Int());
            AddColumn("dbo.CustomerAccounts", "LoanAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CustomerAccounts", "LinkedAccountID", c => c.Int());
            CreateIndex("dbo.CustomerAccounts", "LinkedAccountID");
            AddForeignKey("dbo.CustomerAccounts", "LinkedAccountID", "dbo.CustomerAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerAccounts", "LinkedAccountID", "dbo.CustomerAccounts");
            DropIndex("dbo.CustomerAccounts", new[] { "LinkedAccountID" });
            DropColumn("dbo.CustomerAccounts", "LinkedAccountID");
            DropColumn("dbo.CustomerAccounts", "LoanAmount");
            DropColumn("dbo.CustomerAccounts", "TermsOfLoan");
            DropColumn("dbo.CustomerAccounts", "LoanPrincipalRemaining");
            DropColumn("dbo.CustomerAccounts", "LoanMonthlyPrincipalRepay");
            DropColumn("dbo.CustomerAccounts", "LoanMonthlyRepay");
            DropColumn("dbo.CustomerAccounts", "LoanMonthlyInterestRepay");
        }
    }
}
