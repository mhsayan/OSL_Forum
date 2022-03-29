using System;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class FavoriteForumRepository : Repository<FavoriteForum, Guid>, IFavoriteForumRepository
    {
        public FavoriteForumRepository(ICoreDbContext dbContext)
            : base((CoreDbContext)dbContext)
        {

        }
    }
}