using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;

namespace OSL.Forum.Core.Repositories
{
    public class ForumRepository : Repository<Entities.Forum, long>, IForumRepository
    {
        private static ForumRepository _forumRepository;
        private readonly ICoreDbContext _dbContext;

        private ForumRepository(ICoreDbContext dbContext)
        {
            _dbContext = dbContext;

            base.Resolve((DbContext)_dbContext);
        }

        public static ForumRepository Create(ICoreDbContext dbContext)
        {
            //if (_forumRepository == null)
            //{
            //    _forumRepository = new ForumRepository(dbContext);
            //}

            return new ForumRepository(dbContext);
        }
    }
}