using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;

namespace OSL.Forum.Core.Repositories
{
    public class ForumRepository : Repository<Entities.Forum, long>, IForumRepository
    {
        private readonly ICoreDbContext _dbContext;

        public ForumRepository(ICoreDbContext dbContext)
        {
            _dbContext = dbContext;

            base.Resolve((DbContext)_dbContext);
        }
    }
}