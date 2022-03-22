using System;
using OSL.Forum.Base;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public interface IForumRepository : IRepository<Entities.Forum, Guid>
    {

    }
}