using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using OSL.Forum.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using NHibernate.AspNet.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.AspNet.Identity.Helpers;
using OSL.Forum.NHibernate.Core.Convention;
using OSL.Forum.NHibernate.Core.Mappings;

namespace OSL.Forum.NHibernate.Core.Contexts
{
    public class NHibernateCoreDbContext
    {
        private static ISessionFactory _session;

        private static ISessionFactory CreateSession()
        {
            if (_session != null)
            {
                return _session;
            }

            var connectionString = ConfigurationManager.ConnectionStrings["NHibernateConnection"].ToString();

            FluentConfiguration _config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.Conventions.AddFromAssemblyOf<TableNameConvention>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CategoryMapping>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ForumMapping>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TopicMapping>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PostMapping>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<FavoriteForumMapping>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.SessionFactory().DefaultFlushMode(FlushMode.Commit);
                    new SchemaUpdate(cfg).Execute(false, true);
                });

            _session = _config.BuildSessionFactory();

            return _session;
        }

        public static ISession GetSession()
        {
            return CreateSession().OpenSession();
        }
    }
}
