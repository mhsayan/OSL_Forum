using System;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class TopicRepository : Repository<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(ICoreDbContext dbContext)
            : base((CoreDbContext)dbContext)
        {

        }
    }
}