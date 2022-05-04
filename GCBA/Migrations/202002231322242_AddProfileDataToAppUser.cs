namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileDataToAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "RoleID", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "BranchID", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "RoleID");
            CreateIndex("dbo.AspNetUsers", "BranchID");
            AddForeignKey("dbo.AspNetUsers", "RoleID", "dbo.Roles", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "BranchID", "dbo.Branches", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.AspNetUsers", "BranchID", "dbo.Branches");
            DropIndex("dbo.AspNetUsers", new[] { "RoleID" });
            DropIndex("dbo.AspNetUsers", new[] { "BranchID" });
            DropColumn("dbo.AspNetUsers", "RoleID");
            DropColumn("dbo.AspNetUsers", "BranchID");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}
