using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Event.Default;
using NHibernate.Linq;

namespace OSL.Forum.NHibernateBase
{
    public abstract class Repository<TEntity, TKey>
        : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public virtual void Add(TEntity entity)
        {
            _session.Save(entity);
        }

        public virtual void Remove(TKey id)
        {
            var entityToDelete = _session.Get<TEntity>(id);
            Remove(entityToDelete);
        }

        public virtual void Remove(TEntity entityToDelete)
        {
            _session.Delete(entityToDelete);
        }

        public virtual void Edit(TEntity entityToUpdate)
        {
            _session.Update(entityToUpdate);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();
            var count = 0;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            count = query.Count();
            return count;
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Fetch(x => x.GetType().GetProperty(includeProperty));
            }

            return query.ToList();
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "",
            int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Fetch(x => x.GetType().GetProperty(includeProperty));
            }

            var result = query.OrderByDescending(s => s.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return result;
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Fetch(x => x.GetType().GetProperty(includeProperty));
            }

            if (orderBy != null)
            {
                return orderBy(query).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return query.OrderByDescending(s => s.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public virtual IList<TEntity> GetAll()
        {
            return _session.Query<TEntity>().ToList();
        }

        public virtual TEntity GetById(TKey id)
        {
            return _session.Get<TEntity>(id);
        }

        public virtual (IList<TEntity> data, int total, int totalDisplay) GetDynamic(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();
            var total = query.Count();
            var totalDisplay = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = query.Count();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Fetch(x => x.GetType().GetProperty(includeProperty));
            }

            if (orderBy != null)
            {
                var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return (result.ToList(), total, totalDisplay);
            }
            else
            {
                var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return (result.ToList(), total, totalDisplay);
            }
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Fetch(x => x.GetType().GetProperty(includeProperty));
            }

            if (orderBy != null)
            {
                var result = orderBy(query);
                return result.ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IList<TEntity> GetDynamic(Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _session.Query<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Fetch(x => x.GetType().GetProperty(includeProperty));
            }

            if (orderBy != null)
            {
                var result = query.OrderBy(orderBy);
                return result.ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}
