using System;
using System.Collections.Generic;
using OSL.Forum.Entities;

namespace OSL.Forum.DAO
{
    public interface ITopicRepository : IDisposable
    {
        void Save();
        Topic Get(string topicName, long forumId);
        Topic GetWithIncludedProperty(long topicId, string includeProperty = "");
        Topic GetByName(string topicName);
        Topic GetById(long topicId);
        void RemoveById(long topicId);
        void Add(Topic topic);
        long GetCount();
        long GetCountByForumId(long forumId);
        IList<Topic> Load(long forumId, int pagerCurrentPage, int pagerPageSize, bool tracking);
    }
}