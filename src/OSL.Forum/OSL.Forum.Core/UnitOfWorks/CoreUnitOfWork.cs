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
        private static CoreUnitOfWork _coreUnitOfWork;

        private ICategoryRepository Categories { get; set; }
        private IForumRepository Forums { get; set; }
        private IPostRepository Posts { get; set; }
        private ITopicRepository Topics { get; set; }
        private IFavoriteForumRepository FavoriteForums { get; set; }

        private CoreUnitOfWork()
        {
            base.Resolve(new CoreDbContext());
        }

        public static CoreUnitOfWork Create()
        {
            if (_coreUnitOfWork == null)
            {
                _coreUnitOfWork = new CoreUnitOfWork()
                {
                    Categories = CategoryRepository.Create(),
                    Forums = ForumRepository.Create(),
                    Posts = PostRepository.Create(),
                    Topics = TopicRepository.Create(),
                    FavoriteForums = FavoriteForumRepository.Create(),

                };
            }

            return _coreUnitOfWork;
        }
    }
}
