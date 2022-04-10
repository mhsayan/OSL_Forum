using System;
using NHibernate;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public class ForumRepository : Repository<Entities.Forum, Guid>, IForumRepository
    {
        public ForumRepository(ISession session)
            : base(session)
        {

        }
    }
}