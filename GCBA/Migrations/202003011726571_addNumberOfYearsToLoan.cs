namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNumberOfYearsToLoan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConsumerAccounts", "NumberOfYears", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConsumerAccounts", "NumberOfYears");
        }
    }
}
