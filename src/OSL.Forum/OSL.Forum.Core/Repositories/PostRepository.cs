using System;
using System.Data.Entity;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public class PostRepository : Repository<Post, long>, IPostRepository
    {
        private static PostRepository _postRepository;
        private readonly ICoreDbContext _dbContext;

        private PostRepository()
        {
            _dbContext = CoreDbContext.Create();

            base.Resolve((DbContext)_dbContext);
        }

        public static PostRepository Create()
        {
            if (_postRepository == null)
            {
                _postRepository = new PostRepository();
            }

            return _postRepository;
        }
    }
}