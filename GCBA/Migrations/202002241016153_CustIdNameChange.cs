namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustIdNameChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CustomerLongId", c => c.String());
            DropColumn("dbo.Customers", "CustId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "CustId", c => c.String());
            DropColumn("dbo.Customers", "CustomerLongId");
        }
    }
}
