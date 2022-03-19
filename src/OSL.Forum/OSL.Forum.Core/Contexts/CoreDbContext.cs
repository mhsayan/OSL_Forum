using System.Data.Entity;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Contexts
{
    public class CoreDbContext : DbContext, ICoreDbContext
    {

        public CoreDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        DbSet<Category> Categories { get; set; }
    }
}