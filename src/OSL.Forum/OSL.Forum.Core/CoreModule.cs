using Autofac;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Repositories;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.UnitOfWorks;
using OSL.Forum.Core.Utilities;

namespace OSL.Forum.Core
{
    public class CoreModule : Module
    {
        private readonly string _connectionString;

        public CoreModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CoreDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<CoreDbContext>().As<ICoreDbContext>()
                .WithParameter("connectionString", _connectionString)
                .InstancePerLifetimeScope();

            //Utilities
            builder.RegisterType<DateTimeUtility>().As<IDateTimeUtility>()
                .InstancePerLifetimeScope();

            //Repositories
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ForumRepository>().As<IForumRepository>()
                .InstancePerLifetimeScope();

            //Unit Of Work
            builder.RegisterType<CoreUnitOfWork>().As<ICoreUnitOfWork>().InstancePerLifetimeScope();

            //Service Classes
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ForumService>().As<IForumService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}