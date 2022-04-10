﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OSL.Forum.NHibernateBase
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        void Add(TEntity entity);
        void Remove(TKey id);
        void Remove(TEntity entityToDelete);
        void Edit(TEntity entityToUpdate);
        int GetCount(Expression<Func<TEntity, bool>> filter = null);
        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        IList<TEntity> GetAll();
        TEntity GetById(TKey id);
        (IList<TEntity> data, int total, int totalDisplay) GetDynamic(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null, int pageIndex = 1, int pageSize = 10);

        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter, int pageIndex = 1,
            int pageSize = 10);

        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null, int pageIndex = 1, int pageSize = 10);

        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IList<TEntity> GetDynamic(Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null);
    }
}