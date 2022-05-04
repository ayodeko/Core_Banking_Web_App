namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGlAccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlAccounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountName = c.String(nullable: false),
                        Code = c.Long(nullable: false),
                        AccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchID = c.Int(nullable: false),
                        GlCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.BranchID, cascadeDelete: true)
                .ForeignKey("dbo.GlCategories", t => t.GlCategoryID, cascadeDelete: true)
                .Index(t => t.BranchID)
                .Index(t => t.GlCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GlAccounts", "GlCategoryID", "dbo.GlCategories");
            DropForeignKey("dbo.GlAccounts", "BranchID", "dbo.Branches");
            DropIndex("dbo.GlAccounts", new[] { "GlCategoryID" });
            DropIndex("dbo.GlAccounts", new[] { "BranchID" });
            DropTable("dbo.GlAccounts");
        }
    }
}
