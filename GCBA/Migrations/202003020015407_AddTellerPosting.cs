namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTellerPosting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TellerPostings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Narration = c.String(),
                        Date = c.DateTime(nullable: false),
                        PostingType = c.Int(nullable: false),
                        ConsumerAccountID = c.Int(nullable: false),
                        PostInitiatorId = c.String(),
                        TillAccountID = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ConsumerAccounts", t => t.ConsumerAccountID, cascadeDelete: true)
                .ForeignKey("dbo.GlAccounts", t => t.TillAccountID)
                .Index(t => t.ConsumerAccountID)
                .Index(t => t.TillAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TellerPostings", "TillAccountID", "dbo.GlAccounts");
            DropForeignKey("dbo.TellerPostings", "ConsumerAccountID", "dbo.ConsumerAccounts");
            DropIndex("dbo.TellerPostings", new[] { "TillAccountID" });
            DropIndex("dbo.TellerPostings", new[] { "ConsumerAccountID" });
            DropTable("dbo.TellerPostings");
        }
    }
}
