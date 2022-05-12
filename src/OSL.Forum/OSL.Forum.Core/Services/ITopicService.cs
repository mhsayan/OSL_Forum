using System;
using System.Collections.Generic;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
{
    public interface ITopicService
    {
        void CreateTopic(BO.Topic topic);
        BO.Topic GetTopic(string topicName, long forumId);
        BO.Topic GetTopic(long topicId);
        BO.Topic GetTopic(string topicName);
        void UpdateModificationDate(long topicId, DateTime time);
        void UpdateTopicApprovalType(long topicId);
        long GetTopicCount(long id);
        IList<BO.Topic> GetTopics(int pagerCurrentPage, int pagerPageSize, long forumId);
        void DeleteTopic(long topicId);
        void CloseTopic(long topicId);
        void OpenTopic(long topicId);
    }
}
