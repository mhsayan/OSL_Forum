using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;

namespace OSL.Forum.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        public CoreUnitOfWork(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
