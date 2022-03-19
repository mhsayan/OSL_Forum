using Autofac;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Repositories;
using OSL.Forum.Core.UnitOfWorks;

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

            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CoreUnitOfWork>().As<ICoreUnitOfWork>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}