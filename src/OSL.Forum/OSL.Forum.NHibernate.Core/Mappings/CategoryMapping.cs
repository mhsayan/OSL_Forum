using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using OSL.Forum.NHibernate.Core.Entities;


namespace OSL.Forum.NHibernate.Core.Mappings
{
    public class CategoryMapping : ClassMap<Category>
    {
        public CategoryMapping()
        {
            Table("Categories");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.Name).Length(64).Not.Nullable();
            Map(c => c.CreationDate).Not.Nullable();
            Map(c => c.ModificationDate).Not.Nullable();
            HasMany(c => c.Forums).Cascade.SaveUpdate().Not.LazyLoad().Inverse();
        }
    }
}
