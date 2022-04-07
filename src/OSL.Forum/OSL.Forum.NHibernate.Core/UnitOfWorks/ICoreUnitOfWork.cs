using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.NHibernate.Core.Repositories;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.UnitOfWorks
{
    public interface ICoreUnitOfWork : IUnitOfWork
    {
        ICategoryRepository Categories { get; }
    }
}
