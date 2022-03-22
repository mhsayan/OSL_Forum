namespace OSL.Forum.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFixedTableNameForEntities : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fora", newName: "Forums");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Forums", newName: "Fora");
        }
    }
}
