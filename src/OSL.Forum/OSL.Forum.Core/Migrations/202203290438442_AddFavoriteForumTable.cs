namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddFavoriteForumTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavoriteForums",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                    ApplicationUserId = c.String(),
                    ForumId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forums", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FavoriteForums", "ForumId", "dbo.Forums");
            DropIndex("dbo.FavoriteForums", new[] { "ForumId" });
            DropTable("dbo.FavoriteForums");
        }
    }
}
