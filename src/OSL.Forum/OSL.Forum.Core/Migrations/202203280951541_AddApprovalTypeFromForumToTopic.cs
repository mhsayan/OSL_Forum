namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApprovalTypeFromForumToTopic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "ApprovalType", c => c.String());
            DropColumn("dbo.Forums", "ApprovalType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Forums", "ApprovalType", c => c.String());
            DropColumn("dbo.Topics", "ApprovalType");
        }
    }
}
