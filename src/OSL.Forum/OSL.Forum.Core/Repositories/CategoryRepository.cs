﻿using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class CategoryRepository : Repository<Category, long>, ICategoryRepository
    {
        private readonly ICoreDbContext _dbContext;

        public CategoryRepository(ICoreDbContext dbContext)
        {
            _dbContext = dbContext;

            base.Resolve((DbContext)_dbContext);
        }
    }
}