using System;
using NHibernate;
using OSL.Forum.NHibernateBase;
using OSL.Forum.NHibernate.Core.Contexts;
using OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(ISession session)
            : base(session)
        {

        }
    }
}