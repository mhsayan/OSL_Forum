using System;
using NHibernate;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public class TopicRepository : Repository<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(ISession session)
            : base(session)
        {

        }
    }
}