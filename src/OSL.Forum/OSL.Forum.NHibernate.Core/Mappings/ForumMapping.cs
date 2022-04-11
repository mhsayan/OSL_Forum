using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace OSL.Forum.NHibernate.Core.Mappings
{
    public class ForumMapping : ClassMap<Entities.Forum>
    {
        public ForumMapping()
        {
            Table("Forums");
            Id(f => f.Id).GeneratedBy.GuidComb();
            Map(f => f.Name).Length(64).Not.Nullable();
            Map(f => f.CreationDate).Not.Nullable();
            Map(f => f.ModificationDate).Not.Nullable();
            Map(f => f.ApplicationUserId).Formula("[ApplicationUser_id]").Not.Nullable();
            References(f => f.ApplicationUser);
            Map(f => f.CategoryId).Formula("[Category_id]").Not.Nullable();
            References(f => f.Category);
            HasMany(f => f.Topics).Cascade.AllDeleteOrphan();
        }
    }
}
