using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using OSL.Forum.Membership.Entities;

namespace OSL.Forum.Membership.Mappings
{
    public class ApplicationUserMapping : SubclassMap<ApplicationUser>
    {
        public ApplicationUserMapping()
        {
            Table("AspNetUsers");
            Map(x => x.Name);
        }
    }
}
