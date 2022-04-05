using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.UnitOfWorks
{
    public class CoreUnitOfWork : UnitOfWork, ICoreUnitOfWork
    {
        public ICategoryRepository Categories { get; private set; }
        public IForumRepository Forums { get; private set; }
        public IPostRepository Posts { get; private set; }
        public ITopicRepository Topics { get; private set; }
        public IFavoriteForumRepository FavoriteForums { get; private set; }

        public CoreUnitOfWork(ICoreDbContext context,
            ICategoryRepository categories, IForumRepository forums,
            IPostRepository posts, ITopicRepository topics,
            IFavoriteForumRepository favoriteForums
            ) : base((DbContext)context)
        {
            Categories = categories;
            Forums = forums;
            Posts = posts;
            Topics = topics;
            FavoriteForums = favoriteForums;
        }
    }
}
