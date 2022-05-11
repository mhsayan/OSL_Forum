using System;
using System.Collections.Generic;
using OSL.Forum.Base;

namespace OSL.Forum.Core.Repositories
{
    public interface IForumRepository : IDisposable
    {
        void Save();
        Entities.Forum Get(string forumName, long categoryId);
        Entities.Forum GetWithIncludedProperty(long forumId, string includeProperty = "");
        Entities.Forum GetByName(string forumName);
        Entities.Forum GetById(long forumId);
        void RemoveById(long forumId);
        void Add(Entities.Forum forum);
        long GetCount();
        long GetByCategoryId(long categoryId);
        IList<Entities.Forum> Load(long categoryId, int pagerCurrentPage, int pagerPageSize, bool b);
    }
}