using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.AspNet.Identity;
using OSL.Forum.Membership.Contexts;
using OSL.Forum.Membership.Entities;

namespace OSL.Forum.Membership
{
    public class MembershipModule : Module
    {
        private readonly string _connectionString;

        public MembershipModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(s => MembershipDbContext.GetSession()).As<ISession>().InstancePerLifetimeScope();
            builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>();
            builder.RegisterType<UserManager<ApplicationUser>>();

            base.Load(builder);
        }
    }
}
