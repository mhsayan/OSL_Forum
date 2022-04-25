using System;
using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class CategoryRepository : Repository<Category, long>, ICategoryRepository
    {
        private static CategoryRepository _categoryRepository;
        private readonly ICoreDbContext _dbContext;

        private CategoryRepository()
        {
            _dbContext = CoreDbContext.Create();

            base.Resolve((DbContext)_dbContext);
        }

        public static CategoryRepository Create()
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new CategoryRepository();
            }

            return _categoryRepository;
        }
    }
}