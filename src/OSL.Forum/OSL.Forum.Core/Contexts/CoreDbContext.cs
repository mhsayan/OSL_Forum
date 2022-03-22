using System.Data.Entity;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Contexts
{
    public class CoreDbContext : DbContext, ICoreDbContext
    {
        public CoreDbContext() : base("DefaultConnection")
        {

        }

        public CoreDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id)
                .HasMany(c => c.Forums);

            modelBuilder.Entity<Entities.Forum>()
                .HasKey(f => f.Id)
                .HasOptional(f => f.Category)
                .WithMany(c => c.Forums)
                .HasForeignKey(f => f.CategoryId);

            modelBuilder.Entity<Topic>()
                .HasKey(t => t.Id)
                .HasOptional(t => t.Forum)
                .WithMany(f => f.Topics)
                .HasForeignKey(t => t.ForumId);

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id)
                .HasOptional(p => p.Topic)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => p.TopicId);

            base.OnModelCreating(modelBuilder);
        }

        DbSet<Category> Categories { get; set; }
    }
}