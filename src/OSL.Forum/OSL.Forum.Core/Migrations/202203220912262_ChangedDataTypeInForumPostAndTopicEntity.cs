namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDataTypeInForumPostAndTopicEntity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Forums", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Topics", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Posts", "ApplicationUserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "ApplicationUserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Topics", "ApplicationUserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Forums", "ApplicationUserId", c => c.Guid(nullable: false));
        }
    }
}
