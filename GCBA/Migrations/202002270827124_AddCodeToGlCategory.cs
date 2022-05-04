namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeToGlCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlCategories", "Code", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlCategories", "Code");
        }
    }
}
