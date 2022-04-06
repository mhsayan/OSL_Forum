using System;
using OSL.Forum.NHibernateBase;
using OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {

    }
}