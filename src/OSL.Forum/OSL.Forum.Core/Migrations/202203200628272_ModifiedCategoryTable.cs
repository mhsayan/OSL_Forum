namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Categories", "ModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Categories", "Name", c => c.String(maxLength: 64));
            DropColumn("dbo.Categories", "CreateTime");
            DropColumn("dbo.Categories", "EditTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "EditTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Categories", "CreateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Categories", "Name", c => c.String());
            DropColumn("dbo.Categories", "ModificationDate");
            DropColumn("dbo.Categories", "CreationDate");
        }
    }
}
