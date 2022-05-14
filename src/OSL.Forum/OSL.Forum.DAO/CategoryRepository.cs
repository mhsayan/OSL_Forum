using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OSL.Forum.Entities;
using OSL.Forum.Entities.Contexts;

namespace OSL.Forum.DAO
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Category>();
        }

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
        }

        public virtual void Save()
        {
            _dbContext?.SaveChanges();
        }

        public virtual Category GetByName(string name)
        {
            IQueryable<Category> query = _dbSet;

            return query.FirstOrDefault(c => c.Name == name);
        }

        public virtual Category GetById(long categoryId)
        {
            IQueryable<Category> query = _dbSet;

            return query.FirstOrDefault(c => c.Id == categoryId);
        }

        public virtual void RemoveById(long categoryId)
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

        public virtual void Add(Category category)
        {
            _dbSet.Add(category);
        }

        public virtual long GetCount()
        {
           return _dbSet.Count();
        }

        public virtual IList<Category> Load(int pageIndex, int pageSize, bool tracking, string includedProperty = "")
        {
            IQueryable<Category> query = _dbSet.Include(includedProperty);

            var result = query.OrderByDescending(c => c.ModificationDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }
    }
}