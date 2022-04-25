using System;
using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class TopicRepository : Repository<Topic, long>, ITopicRepository
    {
        private static TopicRepository _topicRepository;
        private readonly ICoreDbContext _dbContext;

        public TopicRepository()
        {
            _dbContext = CoreDbContext.Create();

            base.Resolve((DbContext)_dbContext);
        }

        public static TopicRepository Create()
        {
            if (_topicRepository == null)
            {
                _topicRepository = new TopicRepository();
            }

            return _topicRepository;
        }
    }
}