namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedApprovalTypeInForum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fora", "ApprovalType", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Fora", "ApprovalType");
        }
    }
}
