using System.Data;
using NHibernate;

namespace OSL.Forum.NHibernateBase
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ISession _session;
        protected ITransaction _transaction;

        public UnitOfWork(ISession session)
        {
            _session = session;
            _transaction = _session.BeginTransaction();
        }

        public void Save()
        {
            try
            {
                _transaction = _session.BeginTransaction();
                _transaction.Commit();
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                {
                    _transaction.Rollback();
                }
            }
        }

        public void Rollback()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }
    }
}
