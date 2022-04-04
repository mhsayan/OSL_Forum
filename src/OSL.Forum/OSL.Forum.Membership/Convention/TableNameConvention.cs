using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using Pluralize.NET;

namespace OSL.Forum.Membership.Convention
{
    public class TableNameConvention : IClassConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
        {
            string typeName = instance.EntityType.Name;

            IPluralize pluralize = new Pluralizer();

            instance.Table(pluralize.Pluralize(typeName));
        }
    }
}
