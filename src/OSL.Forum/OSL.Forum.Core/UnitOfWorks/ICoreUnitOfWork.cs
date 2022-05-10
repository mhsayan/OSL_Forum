using OSL.Forum.Base;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.UnitOfWorks
{
    public interface ICoreUnitOfWork : IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IForumRepository Forums { get; }
        IPostRepository Posts { get; }
        ITopicRepository Topics { get; }
        IFavoriteForumRepository FavoriteForums { get; }
    }
}
