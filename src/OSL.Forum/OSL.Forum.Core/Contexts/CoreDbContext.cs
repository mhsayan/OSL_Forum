using System.Data.Entity;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Contexts
{
    public class CoreDbContext : DbContext, ICoreDbContext
    {
        private static CoreDbContext _coreDbContext;

        public CoreDbContext() : base("DefaultConnection")
        {

        }

        public static CoreDbContext Create()
        {
            if (_coreDbContext == null)
                _coreDbContext = new CoreDbContext();

            return _coreDbContext;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id)
                .HasMany(c => c.Forums);

            modelBuilder.Entity<Entities.Forum>()
                .HasKey(f => f.Id)
                .HasRequired(f => f.Category)
                .WithMany(c => c.Forums)
                .HasForeignKey(f => f.CategoryId);

            modelBuilder.Entity<Topic>()
                .HasKey(t => t.Id)
                .HasRequired(t => t.Forum)
                .WithMany(f => f.Topics)
                .HasForeignKey(t => t.ForumId);

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id)
                .HasRequired(p => p.Topic)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => p.TopicId);

            base.OnModelCreating(modelBuilder);
        }

        DbSet<Category> Categories { get; set; }
        DbSet<Entities.Forum> Forums { get; set; }
        DbSet<Topic> Topics { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<FavoriteForum> FavoriteForums { get; set; }
    }
}