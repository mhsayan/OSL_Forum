using System;
using System.Collections.Generic;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.NHibernate.Core.Services
{
    public interface ITopicService
    {
        void CreateTopic(BO.Topic topic);
        BO.Topic GetTopic(string topicName, Guid forumId);
        BO.Topic GetTopic(Guid topicId);
        BO.Topic GetTopic(string topicName);
        void UpdateModificationDate(Guid topicId, DateTime time);
        void UpdateTopicApprovalType(Guid topicId);
        int GetTopicCount(Guid id);
        IList<BO.Topic> GetTopics(int pagerCurrentPage, int pagerPageSize, Guid forumId);
        void DeleteTopic(Guid topicId);
        void CloseTopic(Guid topicId);
        void OpenTopic(Guid topicId);
    }
}
