using System;

namespace OSL.Forum.NHibernateBase
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
