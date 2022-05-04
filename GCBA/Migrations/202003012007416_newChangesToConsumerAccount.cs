namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newChangesToConsumerAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConsumerAccounts", "DaysCount", c => c.Int());
            AddColumn("dbo.ConsumerAccounts", "dailyInterestAccrued", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ConsumerAccounts", "LoanInterestRatePerMonth", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ConsumerAccounts", "SavingsWithdrawalCount", c => c.Int());
            AddColumn("dbo.ConsumerAccounts", "CurrentLien", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConsumerAccounts", "CurrentLien");
            DropColumn("dbo.ConsumerAccounts", "SavingsWithdrawalCount");
            DropColumn("dbo.ConsumerAccounts", "LoanInterestRatePerMonth");
            DropColumn("dbo.ConsumerAccounts", "dailyInterestAccrued");
            DropColumn("dbo.ConsumerAccounts", "DaysCount");
        }
    }
}
