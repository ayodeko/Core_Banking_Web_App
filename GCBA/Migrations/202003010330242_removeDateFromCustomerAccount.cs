namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeDateFromCustomerAccount : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomerAccounts", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerAccounts", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
