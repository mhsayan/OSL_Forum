using System;

namespace OSL.Forum.NHibernateBase
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
