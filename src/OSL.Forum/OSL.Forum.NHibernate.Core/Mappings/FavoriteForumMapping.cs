﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;
using OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Mappings
{
    public class FavoriteForumMapping : ClassMap<FavoriteForum>
    {
        public FavoriteForumMapping()
        {
            Table("FavoriteForums");
            Id(ff => ff.Id).GeneratedBy.GuidComb();
            Map(ff => ff.ApplicationUserId).Formula("[ApplicationUser_id]").Not.Nullable();
            References(ff => ff.ApplicationUser);
            Map(ff => ff.ForumId).Formula("[Forum_id]").Not.Nullable();
            References(ff => ff.Forum);
        }
    }
}
