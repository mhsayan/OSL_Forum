namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Forums",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                    Name = c.String(nullable: false, maxLength: 64),
                    CreationDate = c.DateTime(nullable: false),
                    ModificationDate = c.DateTime(nullable: false),
                    CategoryId = c.Guid(nullable: false),
                    ApplicationUserId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);

            CreateTable(
                "dbo.Topics",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                    Name = c.String(nullable: false, maxLength: 64),
                    CreationDate = c.DateTime(nullable: false),
                    ModificationDate = c.DateTime(nullable: false),
                    ForumId = c.Guid(nullable: false),
                    ApplicationUserId = c.Guid(nullable: false),
                    Status = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forums", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId);

            CreateTable(
                "dbo.Posts",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                    Name = c.String(nullable: false, maxLength: 64),
                    Description = c.String(nullable: false),
                    CreationDate = c.DateTime(nullable: false),
                    ModificationDate = c.DateTime(nullable: false),
                    TopicId = c.Guid(nullable: false),
                    ApplicationUserId = c.Guid(nullable: false),
                    Status = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);

            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 64));
        }

        public override void Down()
        {
            DropForeignKey("dbo.Posts", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Topics", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Forums", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Posts", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "ForumId" });
            DropIndex("dbo.Forums", new[] { "CategoryId" });
            AlterColumn("dbo.Categories", "Name", c => c.String(maxLength: 64));
            DropTable("dbo.Posts");
            DropTable("dbo.Topics");
            DropTable("dbo.Forums");
        }
    }
}
