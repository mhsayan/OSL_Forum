using System;

namespace OSL.Forum.Base
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
