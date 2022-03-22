using System;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class ForumRepository : Repository<Entities.Forum, Guid>, IForumRepository
    {
        public ForumRepository(ICoreDbContext dbContext)
            : base((CoreDbContext)dbContext)
        {

        }
    }
}