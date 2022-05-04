namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "MembershipTypeId", "dbo.MembershipTypes");
            DropIndex("dbo.Customers", new[] { "MembershipTypeId" });
            AddColumn("dbo.Customers", "CustId", c => c.String());
            AddColumn("dbo.Customers", "FullName", c => c.String(nullable: false, maxLength: 40));
            AddColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Customers", "Email", c => c.String(maxLength: 100));
            AddColumn("dbo.Customers", "PhoneNumber", c => c.String(nullable: false, maxLength: 16));
            AddColumn("dbo.Customers", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "Name");
            DropColumn("dbo.Customers", "BirthDay");
            DropColumn("dbo.Customers", "IsSubscribedToNewsletter");
            DropColumn("dbo.Customers", "MembershipTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "MembershipTypeId", c => c.Byte(nullable: false));
            AddColumn("dbo.Customers", "IsSubscribedToNewsletter", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Customers", "Status");
            DropColumn("dbo.Customers", "Gender");
            DropColumn("dbo.Customers", "PhoneNumber");
            DropColumn("dbo.Customers", "Email");
            DropColumn("dbo.Customers", "Address");
            DropColumn("dbo.Customers", "FullName");
            DropColumn("dbo.Customers", "CustId");
            CreateIndex("dbo.Customers", "MembershipTypeId");
            AddForeignKey("dbo.Customers", "MembershipTypeId", "dbo.MembershipTypes", "Id", cascadeDelete: true);
        }
    }
}
