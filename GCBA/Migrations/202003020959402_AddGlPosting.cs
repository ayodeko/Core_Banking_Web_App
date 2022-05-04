namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGlPosting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlPostings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Narration = c.String(),
                        Date = c.DateTime(nullable: false),
                        DrGlAccountID = c.Int(),
                        CrGlAccountID = c.Int(),
                        PostInitiatorId = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GlAccounts", t => t.CrGlAccountID)
                .ForeignKey("dbo.GlAccounts", t => t.DrGlAccountID)
                .Index(t => t.DrGlAccountID)
                .Index(t => t.CrGlAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GlPostings", "DrGlAccountID", "dbo.GlAccounts");
            DropForeignKey("dbo.GlPostings", "CrGlAccountID", "dbo.GlAccounts");
            DropIndex("dbo.GlPostings", new[] { "CrGlAccountID" });
            DropIndex("dbo.GlPostings", new[] { "DrGlAccountID" });
            DropTable("dbo.GlPostings");
        }
    }
}
