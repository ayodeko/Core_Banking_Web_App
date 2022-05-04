namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNullableToConsumerAccount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConsumerAccounts", "DaysCount", c => c.Int());
            AlterColumn("dbo.ConsumerAccounts", "dailyInterestAccrued", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ConsumerAccounts", "LoanInterestRatePerMonth", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ConsumerAccounts", "SavingsWithdrawalCount", c => c.Int());
            AlterColumn("dbo.ConsumerAccounts", "CurrentLien", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConsumerAccounts", "CurrentLien", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ConsumerAccounts", "SavingsWithdrawalCount", c => c.Int(nullable: false));
            AlterColumn("dbo.ConsumerAccounts", "LoanInterestRatePerMonth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ConsumerAccounts", "dailyInterestAccrued", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ConsumerAccounts", "DaysCount", c => c.Int(nullable: false));
        }
    }
}
