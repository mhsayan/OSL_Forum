using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository()
        {
            _dbContext = new CoreDbContext();
            _dbSet = _dbContext.Set<Category>();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public void Save()
        {
            _dbContext?.SaveChanges();
        }

        public Category GetByName(string name)
        {
            IQueryable<Category> query = _dbSet;

            return query.FirstOrDefault(c => c.Name == name);
        }

        public Category GetById(long categoryId)
        {
            IQueryable<Category> query = _dbSet;

            return query.FirstOrDefault(c => c.Id == categoryId);
        }

        public void RemoveById(long categoryId)
        {
            var entityToDelete = _dbSet.Find(categoryId);

            if (entityToDelete == null)
                throw new InvalidOperationException("Category not found.");

            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public void Add(Category category)
        {
            _dbSet.Add(category);
        }

        public long GetCount()
        {
           return _dbSet.Count();
        }

        public IList<Category> Load(int pageIndex, int pageSize, bool tracking, string includedProperty = "")
        {
            IQueryable<Category> query = _dbSet.Include(includedProperty);

            var result = query.OrderByDescending(c => c.ModificationDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }
    }
}