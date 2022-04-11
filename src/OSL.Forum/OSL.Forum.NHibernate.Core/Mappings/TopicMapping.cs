using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Mappings
{
    public class TopicMapping : ClassMap<Topic>
    {
        public TopicMapping()
        {
            Table("Topics");
            Id(t => t.Id).GeneratedBy.GuidComb();
            Map(t => t.Name).Length(64).Not.Nullable();
            Map(t => t.CreationDate).Not.Nullable();
            Map(t => t.ModificationDate).Not.Nullable();
            Map(t => t.ApplicationUserId).Formula("[ApplicationUser_id]").Not.Nullable();
            References(t => t.ApplicationUser);
            Map(t => t.ForumId).Formula("[Forum_id]").Not.Nullable();
            References(p => p.Forum);
            Map(t => t.Status).Not.Nullable();
            Map(t => t.ApprovalType).Not.Nullable();
            Map(t => t.ActivityStatus).Not.Nullable();
            HasMany(t => t.Posts).Cascade.AllDeleteOrphan();
        }
    }
}
