using System.Data.Entity.Migrations;
using OSL.Forum.Entities.Contexts;

namespace OSL.Forum.Entities.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CoreDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CoreDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
