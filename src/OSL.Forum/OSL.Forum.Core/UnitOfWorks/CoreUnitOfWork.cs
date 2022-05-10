using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        private static ICoreDbContext _coreDbContext;
        public ICategoryRepository Categories { get; set; }
        public IForumRepository Forums { get; set; }
        public IPostRepository Posts { get; set; }
        public ITopicRepository Topics { get; set; }
        public IFavoriteForumRepository FavoriteForums { get; set; }

        private CoreUnitOfWork()
        {
            _coreDbContext = new CoreDbContext();

            base.Resolve((DbContext)_coreDbContext);
        }

        public static CoreUnitOfWork CreateCategoryRepository()
        {
            return new CoreUnitOfWork()
            {
                Categories = new CategoryRepository(_coreDbContext)
            };
        }

        public static CoreUnitOfWork CreateForumRepository()
        {
            return new CoreUnitOfWork()
            {
                Forums = new ForumRepository(_coreDbContext)
            };
        }

        public static CoreUnitOfWork CreatePostRepository()
        {
            return new CoreUnitOfWork()
            {
                Posts = new PostRepository(_coreDbContext)
            };
        }

        public static CoreUnitOfWork CreateTopicRepository()
        {
            return new CoreUnitOfWork()
            {
                Topics = new TopicRepository(_coreDbContext)
            };
        }

        public static CoreUnitOfWork CreateFavoriteForumRepository()
        {
            return new CoreUnitOfWork()
            {
                FavoriteForums = new FavoriteForumRepository(_coreDbContext)
            };
        }
    }
}
