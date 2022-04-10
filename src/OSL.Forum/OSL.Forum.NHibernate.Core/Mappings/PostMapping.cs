﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Mappings
{
    public class PostMapping : ClassMap<Post>
    {
        public PostMapping()
        {
            Table("Posts");
            Id(p => p.Id).GeneratedBy.GuidComb();
            Map(p => p.Name).Length(64).Not.Nullable();
            Map(p => p.Description).Length(10000).Not.Nullable();
            Map(p => p.CreationDate).Not.Nullable();
            Map(p => p.ModificationDate).Not.Nullable();
            Map(p => p.ApplicationUserId).Not.Nullable();
            References(p => p.ApplicationUser);
            Map(p => p.TopicId).Not.Nullable();
            References(p => p.Topic);
            Map(p => p.Status).Not.Nullable();
        }
    }
}