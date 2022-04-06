using System;
using OSL.Forum.NHibernateBase;
using OSL.Forum.NHibernate.Core.Contexts;
using OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(ICoreDbContext dbContext)
            : base((CoreDbContext)dbContext)
        {

        }
    }
}