using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        private static CoreUnitOfWork _coreUnitOfWork;

        public ICategoryRepository Categories { get; set; }
        public IForumRepository Forums { get; set; }
        public IPostRepository Posts { get; set; }
        public ITopicRepository Topics { get; set; }
        public IFavoriteForumRepository FavoriteForums { get; set; }

        public CoreUnitOfWork()
        {
            base.Resolve(new CoreDbContext());
        }

        public static CoreUnitOfWork Create()
        {
            if (_coreUnitOfWork == null)
            {
                _coreUnitOfWork = new CoreUnitOfWork()
                {
                    Categories = CategoryRepository.Create(),
                    Forums = ForumRepository.Create(),
                    Posts = PostRepository.Create(),
                    Topics = TopicRepository.Create(),
                    FavoriteForums = FavoriteForumRepository.Create()
                };
            }

            return _coreUnitOfWork;
        }
    }
}
