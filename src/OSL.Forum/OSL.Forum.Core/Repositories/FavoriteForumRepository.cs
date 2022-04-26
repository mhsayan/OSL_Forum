using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class FavoriteForumRepository : Repository<FavoriteForum, long>, IFavoriteForumRepository
    {
        private static FavoriteForumRepository _favoriteForumRepository;
        private readonly ICoreDbContext _dbContext;

        private FavoriteForumRepository()
        {
            _dbContext = CoreDbContext.Create();

            base.Resolve((DbContext)_dbContext);
        }

        public static FavoriteForumRepository Create()
        {
            if (_favoriteForumRepository == null)
            {
                _favoriteForumRepository = new FavoriteForumRepository();
            }

            return _favoriteForumRepository;
        }
    }
}