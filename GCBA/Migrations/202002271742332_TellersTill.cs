namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TellersTill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TellersTills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GlAccounId = c.Int(nullable: false),
                        GlAccount_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GlAccounts", t => t.GlAccount_ID)
                .Index(t => t.GlAccount_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TellersTills", "GlAccount_ID", "dbo.GlAccounts");
            DropIndex("dbo.TellersTills", new[] { "GlAccount_ID" });
            DropTable("dbo.TellersTills");
        }
    }
}
