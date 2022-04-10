using Autofac;
using NHibernate;
using OSL.Forum.NHibernate.Core.Contexts;
using OSL.Forum.NHibernate.Core.Repositories;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.NHibernate.Core.UnitOfWorks;
using OSL.Forum.NHibernate.Core.Utilities;

namespace OSL.Forum.NHibernate.Core
{
    public class NHibernateCoreModule : Module
    {
        private readonly string _connectionString;

        public NHibernateCoreModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(s => NHibernateCoreDbContext.GetSession()).As<ISession>().InstancePerLifetimeScope();


            //Utilities
            builder.RegisterType<DateTimeUtility>().As<IDateTimeUtility>()
                .InstancePerLifetimeScope();

            //Repositories
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ForumRepository>().As<IForumRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TopicRepository>().As<ITopicRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<PostRepository>().As<IPostRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FavoriteForumRepository>().As<IFavoriteForumRepository>()
                .InstancePerLifetimeScope();

            //Unit Of Work
            builder.RegisterType<CoreUnitOfWork>().As<ICoreUnitOfWork>().InstancePerLifetimeScope();

            //Service Classes
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ForumService>().As<IForumService>().InstancePerLifetimeScope();
            builder.RegisterType<PostService>().As<IPostService>().InstancePerLifetimeScope();
            builder.RegisterType<TopicService>().As<ITopicService>().InstancePerLifetimeScope();
            builder.RegisterType<FavoriteForumService>().As<IFavoriteForumService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}