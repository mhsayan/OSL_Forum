namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedActivityStatusForTopicEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "ActivityStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "ActivityStatus");
        }
    }
}
