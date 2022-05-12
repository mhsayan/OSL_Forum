using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Post> _dbSet;

        public PostRepository()
        {
            _dbContext = new CoreDbContext();
            _dbSet = _dbContext.Set<Post>();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public void Save()
        {
            _dbContext?.SaveChanges();
        }

        public Post GetById(long topicId)
        {
            IQueryable<Post> query = _dbSet;

            return query.FirstOrDefault(t => t.Id == topicId);
        }

        public IList<Post> LoadByStatus(string status)
        {
            IQueryable<Post> query = _dbSet;

            return query.Where(p => p.Status == status).ToList();
        }

        public IList<Post> LoadByUserId(string applicationUserId)
        {
            IQueryable<Post> query = _dbSet;

            return query.Where(p => p.ApplicationUserId == applicationUserId).ToList();
        }

        public IList<Post> LoadPendingPosts(string status, int pagerCurrentPage, int pagerPageSize, bool tracking)
        {
            IQueryable<Post> query = _dbSet;
            query = query.Where(t => t.Status == status);

            var result = query.OrderBy(c => c.ModificationDate).Skip((pagerCurrentPage - 1) * pagerPageSize).Take(pagerPageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }

        public IList<Post> LoadUserPosts(string userId, int pagerCurrentPage, int pagerPageSize, bool tracking)
        {
            IQueryable<Post> query = _dbSet;
            query = query.Where(t => t.ApplicationUserId == userId);

            var result = query.OrderByDescending(c => c.ModificationDate).Skip((pagerCurrentPage - 1) * pagerPageSize).Take(pagerPageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }

        public void RemoveById(long postId)
        {
            var entityToDelete = _dbSet.Find(postId);

            if (entityToDelete == null)
                throw new InvalidOperationException("Post not found.");

            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public void Add(Post post)
        {
            _dbSet.Add(post);
        }
    }
}