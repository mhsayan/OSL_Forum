using NHibernate;
using OSL.Forum.NHibernateBase;
using OSL.Forum.NHibernate.Core.Repositories;

namespace OSL.Forum.NHibernate.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        public ICategoryRepository Categories { get; private set; }
        public IForumRepository Forums { get; private set; }
        public IPostRepository Posts { get; private set; }
        public ITopicRepository Topics { get; private set; }
        public IFavoriteForumRepository FavoriteForums { get; private set; }

        public CoreUnitOfWork(ISession session,
            ICategoryRepository categories, IForumRepository forums,
            IPostRepository posts, ITopicRepository topics,
            IFavoriteForumRepository favoriteForums
            ) : base(session)
        {
            Categories = categories;
            Forums = forums;
            Posts = posts;
            Topics = topics;
            FavoriteForums = favoriteForums;
        }
    }
}
