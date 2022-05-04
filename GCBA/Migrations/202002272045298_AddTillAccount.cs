namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTillAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TillAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        GlAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GlAccounts", t => t.GlAccountID, cascadeDelete: true)
                .Index(t => t.GlAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TillAccounts", "GlAccountID", "dbo.GlAccounts");
            DropIndex("dbo.TillAccounts", new[] { "GlAccountID" });
            DropTable("dbo.TillAccounts");
        }
    }
}
