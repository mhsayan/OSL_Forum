using NHibernate;
using OSL.Forum.NHibernateBase;
using OSL.Forum.NHibernate.Core.Repositories;

namespace OSL.Forum.NHibernate.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        public ICategoryRepository Categories { get; private set; }

        public CoreUnitOfWork(ISession session,
            ICategoryRepository categories
            ) : base(session)
        {
            Categories = categories;
        }
    }
}
