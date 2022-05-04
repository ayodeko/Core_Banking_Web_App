namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGlCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MainGlCategory = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GlCategories");
        }
    }
}
