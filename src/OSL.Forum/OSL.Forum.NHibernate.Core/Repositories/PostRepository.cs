using System;
using NHibernate;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        public PostRepository(ISession session)
            : base(session)
        {

        }
    }
}