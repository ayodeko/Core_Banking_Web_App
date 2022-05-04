namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TellersTill1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TellersTills", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.TellersTills", "User_Id");
            AddForeignKey("dbo.TellersTills", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TellersTills", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TellersTills", new[] { "User_Id" });
            DropColumn("dbo.TellersTills", "User_Id");
        }
    }
}
