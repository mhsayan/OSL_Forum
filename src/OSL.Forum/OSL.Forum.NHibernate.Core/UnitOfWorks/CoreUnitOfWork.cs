using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.NHibernateBase;
using OSL.Forum.NHibernate.Core.Contexts;
using OSL.Forum.NHibernate.Core.Repositories;

namespace OSL.Forum.NHibernate.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        public ICategoryRepository Categories { get; private set; }

        public CoreUnitOfWork(ICoreDbContext context,
            ICategoryRepository categories
            ) : base((DbContext)context)
        {
            Categories = categories;
        }
    }
}
