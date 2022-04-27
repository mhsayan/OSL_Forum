using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        private static CoreUnitOfWork _coreUnitOfWork;
        private static ICoreDbContext _coreDbContext;

        public ICategoryRepository Categories { get; set; }
        public IForumRepository Forums { get; set; }
        public IPostRepository Posts { get; set; }
        public ITopicRepository Topics { get; set; }
        public IFavoriteForumRepository FavoriteForums { get; set; }

        public CoreUnitOfWork()
        {
            _coreDbContext = new CoreDbContext();

            base.Resolve((DbContext)_coreDbContext);
        }

        public static CoreUnitOfWork Create()
        {
            //if (_coreUnitOfWork == null)
            //{
            //    _coreUnitOfWork = new CoreUnitOfWork()
            //    {
            //        Categories = CategoryRepository.Create(_coreDbContext),
            //        Forums = ForumRepository.Create(_coreDbContext),
            //        Posts = PostRepository.Create(_coreDbContext),
            //        Topics = TopicRepository.Create(_coreDbContext),
            //        FavoriteForums = FavoriteForumRepository.Create(_coreDbContext)
            //    };
            //}

            return _coreUnitOfWork = new CoreUnitOfWork()
            {
                Categories = CategoryRepository.Create(_coreDbContext),
                Forums = ForumRepository.Create(_coreDbContext),
                Posts = PostRepository.Create(_coreDbContext),
                Topics = TopicRepository.Create(_coreDbContext),
                FavoriteForums = FavoriteForumRepository.Create(_coreDbContext)
            };
        }
    }
}
