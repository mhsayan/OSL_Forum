namespace OSL.Forum.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Forums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        ApplicationUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        ForumId = c.Long(nullable: false),
                        ApplicationUserId = c.String(),
                        ApprovalType = c.String(),
                        Status = c.String(),
                        ActivityStatus = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forums", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Description = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        TopicId = c.Long(nullable: false),
                        ApplicationUserId = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.FavoriteForums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ApplicationUserId = c.String(),
                        ForumId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forums", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavoriteForums", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Posts", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Topics", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Forums", "CategoryId", "dbo.Categories");
            DropIndex("dbo.FavoriteForums", new[] { "ForumId" });
            DropIndex("dbo.Posts", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "ForumId" });
            DropIndex("dbo.Forums", new[] { "CategoryId" });
            DropTable("dbo.FavoriteForums");
            DropTable("dbo.Posts");
            DropTable("dbo.Topics");
            DropTable("dbo.Forums");
            DropTable("dbo.Categories");
        }
    }
}
