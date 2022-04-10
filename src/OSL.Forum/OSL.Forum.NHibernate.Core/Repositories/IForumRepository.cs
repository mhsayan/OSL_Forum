using System;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public interface IForumRepository : IRepository<Entities.Forum, Guid>
    {

    }
}