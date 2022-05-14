using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OSL.Forum.Entities;
using OSL.Forum.Entities.Contexts;

namespace OSL.Forum.DAO
{
    public class FavoriteForumRepository : IFavoriteForumRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<FavoriteForum> _dbSet;

        public FavoriteForumRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<FavoriteForum>();
        }

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
        }

        public virtual void Save()
        {
            _dbContext?.SaveChanges();
        }

        public virtual void Add(FavoriteForum favoriteForum)
        {
            _dbSet.Add(favoriteForum);
        }

        public virtual void RemoveById(long favoriteForumId)
        {
            var entityToDelete = _dbSet.Find(favoriteForumId);

            if (entityToDelete == null)
                throw new InvalidOperationException("Favorite Forum not found.");

            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual FavoriteForum Get(long forumId, string userId)
        {
            IQueryable<FavoriteForum> query = _dbSet;

            return query.FirstOrDefault(ff => ff.ForumId == forumId && ff.ApplicationUserId == userId);
        }

        public virtual IList<FavoriteForum> LoadByUserId(string userId)
        {
            IQueryable<FavoriteForum> query = _dbSet;
            query = query.Where(ff => ff.ApplicationUserId == userId);

            return query.ToList();
        }

        public virtual IList<FavoriteForum> Load(string userId, int pageIndex, int pageSize, bool tracking)
        {
            IQueryable<FavoriteForum> query = _dbSet;
            query = query.Where(ff => ff.ApplicationUserId == userId);

            var result = query.OrderBy(ff => ff.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return tracking ? result.AsNoTracking().ToList() : result.ToList();
        }
    }
}