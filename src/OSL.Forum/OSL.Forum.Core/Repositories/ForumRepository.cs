using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;

namespace OSL.Forum.Core.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Entities.Forum> _dbSet;

        public ForumRepository()
        {
            _dbContext = new CoreDbContext();
            _dbSet = _dbContext.Set<Entities.Forum>();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public void Save()
        {
            _dbContext?.SaveChanges();
        }

        public Entities.Forum Get(string forumName, long categoryId)
        {
            IQueryable<Entities.Forum> query = _dbSet;

            return _dbSet.FirstOrDefault(f => f.Name == forumName && f.CategoryId == categoryId);
        }

        public Entities.Forum GetWithIncludedProperty(long forumId, string includeProperty = "")
        {
            IQueryable<Entities.Forum> query = _dbSet.Include(includeProperty);

            return _dbSet.FirstOrDefault(f => f.Id == forumId);
        }

        public Entities.Forum GetByName(string forumName)
        {
            IQueryable<Entities.Forum> query = _dbSet;

            return _dbSet.FirstOrDefault(f => f.Name == forumName);
        }

        public Entities.Forum GetById(long forumId)
        {
            IQueryable<Entities.Forum> query = _dbSet;

            return _dbSet.FirstOrDefault(f => f.Id == forumId);
        }

        public void RemoveById(long forumId)
        {
            var entityToDelete = _dbSet.Find(forumId);

            if (entityToDelete == null)
                throw new InvalidOperationException("Forum not found.");

            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public void Add(Entities.Forum forum)
        {
            _dbSet.Add(forum);
        }

        public long GetCount()
        {
            return _dbSet.Count();
        }

        public long GetByCategoryId(long categoryId)
        {
            return _dbSet.Count(f => f.CategoryId == categoryId);
        }

        public IList<Entities.Forum> Load(long categoryId, int pagerCurrentPage, int pagerPageSize, bool tracking)
        {
            IQueryable<Entities.Forum> query = _dbSet;
            query = query.Where(f => f.CategoryId == categoryId);

            var result = query.OrderByDescending(c => c.ModificationDate).Skip((pagerCurrentPage - 1) * pagerPageSize).Take(pagerPageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }
    }
}