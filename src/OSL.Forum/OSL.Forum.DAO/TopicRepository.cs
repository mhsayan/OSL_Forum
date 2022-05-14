using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OSL.Forum.Entities;
using OSL.Forum.Entities.Contexts;

namespace OSL.Forum.DAO
{
    public class TopicRepository : ITopicRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Topic> _dbSet;

        public TopicRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Topic>();
        }

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
        }

        public virtual void Save()
        {
            _dbContext?.SaveChanges();
        }

        public virtual Topic Get(string topicName, long forumId)
        {
            IQueryable<Topic> query = _dbSet;

            return query.FirstOrDefault(f => f.Name == topicName && f.ForumId == forumId);
        }

        public virtual Topic GetWithIncludedProperty(long topicId, string includeProperty = "")
        {
            IQueryable<Topic> query = _dbSet.Include(includeProperty);

            return query.FirstOrDefault(t => t.Id == topicId);
        }

        public virtual Topic GetByName(string topicName)
        {
            IQueryable<Topic> query = _dbSet;

            return query.FirstOrDefault(t => t.Name == topicName);
        }

        public virtual Topic GetById(long topicId)
        {
            IQueryable<Topic> query = _dbSet;

            return query.FirstOrDefault(t => t.Id == topicId);
        }

        public virtual void RemoveById(long topicId)
        {
            var entityToDelete = _dbSet.Find(topicId);

            if (entityToDelete == null)
                throw new InvalidOperationException("Topic not found.");

            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Add(Topic topic)
        {
            _dbSet.Add(topic);
        }

        public virtual long GetCount()
        {
            return _dbSet.Count();
        }

        public virtual long GetCountByForumId(long forumId)
        {
            return _dbSet.Count(t => t.ForumId == forumId);
        }

        public virtual IList<Topic> Load(long forumId, int pagerCurrentPage, int pagerPageSize, bool tracking)
        {
            IQueryable<Topic> query = _dbSet;
            query = query.Where(f => f.ForumId == forumId);

            var result = query.OrderByDescending(c => c.ModificationDate).Skip((pagerCurrentPage - 1) * pagerPageSize).Take(pagerPageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }
    }
}