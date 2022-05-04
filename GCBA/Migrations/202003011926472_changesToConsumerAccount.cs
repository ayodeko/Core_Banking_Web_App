namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesToConsumerAccount : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ConsumerAccounts", "DaysCount");
            DropColumn("dbo.ConsumerAccounts", "dailyInterestAccrued");
            DropColumn("dbo.ConsumerAccounts", "LoanInterestRatePerMonth");
            DropColumn("dbo.ConsumerAccounts", "SavingsWithdrawalCount");
            DropColumn("dbo.ConsumerAccounts", "CurrentLien");
            DropColumn("dbo.ConsumerAccounts", "NumberOfYears");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConsumerAccounts", "NumberOfYears", c => c.Double(nullable: false));
            AddColumn("dbo.ConsumerAccounts", "CurrentLien", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ConsumerAccounts", "SavingsWithdrawalCount", c => c.Int());
            AddColumn("dbo.ConsumerAccounts", "LoanInterestRatePerMonth", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ConsumerAccounts", "dailyInterestAccrued", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ConsumerAccounts", "DaysCount", c => c.Int());
        }
    }
}
