using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class TopicRepository : Repository<Topic, long>, ITopicRepository
    {
        private readonly ICoreDbContext _dbContext;

        public TopicRepository(ICoreDbContext dbContext)
        {
            _dbContext = dbContext;

            base.Resolve((DbContext)_dbContext);
        }

        public static TopicRepository Create(ICoreDbContext dbContext)
        {
            return new TopicRepository(dbContext);
        }
    }
}