namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedAutoGeneretedIdForCategory : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AddPrimaryKey("dbo.Categories", "Id");
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Categories", "Id");
        }
    }
}
