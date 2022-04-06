using Autofac;
using OSL.Forum.NHibernate.Core.Contexts;
using OSL.Forum.NHibernate.Core.Repositories;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.NHibernate.Core.UnitOfWorks;
using OSL.Forum.NHibernate.Core.Utilities;

namespace OSL.Forum.NHibernate.Core
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

            //Utilities
            builder.RegisterType<DateTimeUtility>().As<IDateTimeUtility>()
                .InstancePerLifetimeScope();

            //Repositories
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope();

            //Unit Of Work
            builder.RegisterType<CoreUnitOfWork>().As<ICoreUnitOfWork>().InstancePerLifetimeScope();

            //Service Classes
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}