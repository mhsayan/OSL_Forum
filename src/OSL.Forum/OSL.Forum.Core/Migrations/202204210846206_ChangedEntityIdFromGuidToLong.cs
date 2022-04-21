namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedEntityIdFromGuidToLong : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Forums", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Topics", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.FavoriteForums", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Posts", "TopicId", "dbo.Topics");
            DropIndex("dbo.Forums", new[] { "CategoryId" });
            DropIndex("dbo.Topics", new[] { "ForumId" });
            DropIndex("dbo.Posts", new[] { "TopicId" });
            DropIndex("dbo.FavoriteForums", new[] { "ForumId" });
            DropPrimaryKey("dbo.Categories");
            DropPrimaryKey("dbo.Forums");
            DropPrimaryKey("dbo.Topics");
            DropPrimaryKey("dbo.Posts");
            DropPrimaryKey("dbo.FavoriteForums");
            AlterColumn("dbo.Categories", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Forums", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Forums", "CategoryId", c => c.Long(nullable: false));
            AlterColumn("dbo.Topics", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Topics", "ForumId", c => c.Long(nullable: false));
            AlterColumn("dbo.Posts", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Posts", "TopicId", c => c.Long(nullable: false));
            AlterColumn("dbo.FavoriteForums", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.FavoriteForums", "ForumId", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Categories", "Id");
            AddPrimaryKey("dbo.Forums", "Id");
            AddPrimaryKey("dbo.Topics", "Id");
            AddPrimaryKey("dbo.Posts", "Id");
            AddPrimaryKey("dbo.FavoriteForums", "Id");
            CreateIndex("dbo.Forums", "CategoryId");
            CreateIndex("dbo.Topics", "ForumId");
            CreateIndex("dbo.Posts", "TopicId");
            CreateIndex("dbo.FavoriteForums", "ForumId");
            AddForeignKey("dbo.Forums", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Topics", "ForumId", "dbo.Forums", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FavoriteForums", "ForumId", "dbo.Forums", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "TopicId", "dbo.Topics", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.FavoriteForums", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Topics", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Forums", "CategoryId", "dbo.Categories");
            DropIndex("dbo.FavoriteForums", new[] { "ForumId" });
            DropIndex("dbo.Posts", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "ForumId" });
            DropIndex("dbo.Forums", new[] { "CategoryId" });
            DropPrimaryKey("dbo.FavoriteForums");
            DropPrimaryKey("dbo.Posts");
            DropPrimaryKey("dbo.Topics");
            DropPrimaryKey("dbo.Forums");
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.FavoriteForums", "ForumId", c => c.Guid(nullable: false));
            AlterColumn("dbo.FavoriteForums", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Posts", "TopicId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Posts", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Topics", "ForumId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Topics", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Forums", "CategoryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Forums", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Categories", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.FavoriteForums", "Id");
            AddPrimaryKey("dbo.Posts", "Id");
            AddPrimaryKey("dbo.Topics", "Id");
            AddPrimaryKey("dbo.Forums", "Id");
            AddPrimaryKey("dbo.Categories", "Id");
            CreateIndex("dbo.FavoriteForums", "ForumId");
            CreateIndex("dbo.Posts", "TopicId");
            CreateIndex("dbo.Topics", "ForumId");
            CreateIndex("dbo.Forums", "CategoryId");
            AddForeignKey("dbo.Posts", "TopicId", "dbo.Topics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FavoriteForums", "ForumId", "dbo.Forums", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Topics", "ForumId", "dbo.Forums", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Forums", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
