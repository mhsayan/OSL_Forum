using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

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


            base.Load(builder);
        }
    }
}
