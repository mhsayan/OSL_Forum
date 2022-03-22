using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.UnitOfWorks
{
    public interface ICoreUnitOfWork : IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IForumRepository Forums { get; }
    }
}
