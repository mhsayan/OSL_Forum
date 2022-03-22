namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedApprovalTypeInForum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Forums", "ApprovalType", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Forums", "ApprovalType");
        }
    }
}
