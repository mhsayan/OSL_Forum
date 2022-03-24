using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
{
    public interface ITopicService
    {
        void CreateTopic(BO.Topic topic);
        BO.Topic GetTopic(string topicName, Guid forumId);
        (BO.Topic topic, List<BO.Post> posts) GetTopic(Guid topicId);
        BO.Topic GetTopic(string topicName);
    }
}
