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

            var myEntities = new[] {
                typeof(ApplicationUser)
            };

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            FluentConfiguration _config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.Conventions.AddFromAssemblyOf<TableNameConvention>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.SessionFactory().DefaultFlushMode(FlushMode.Commit);
                    cfg.AddDeserializedMapping(MappingHelper.GetIdentityMappings(myEntities), null);
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
