using System;
using NHibernate;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public class FavoriteForumRepository : Repository<FavoriteForum, Guid>, IFavoriteForumRepository
    {
        public FavoriteForumRepository(ISession session)
            : base(session)
        {

        }
    }
}