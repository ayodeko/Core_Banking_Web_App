namespace GCBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInfoToConsumer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Consumers", "ConsumerInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Consumers", "ConsumerInfo");
        }
    }
}
