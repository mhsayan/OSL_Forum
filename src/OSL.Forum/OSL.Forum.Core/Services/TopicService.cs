using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class TopicService : ITopicService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public TopicService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Topic GetTopic(string topicName, Guid forumId)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentNullException(nameof(topicName));

            var topicEntity = _unitOfWork.Topics.Get(c =>
                c.Name == topicName && c.ForumId == forumId, "").FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = _mapper.Map<BO.Topic>(topicEntity);

            return topic;
        }

        public BO.Topic GetTopic(Guid topicId)
        {
            if (topicId == Guid.Empty)
                throw new ArgumentNullException(nameof(topicId));

            var topicEntity = _unitOfWork.Topics.Get(c =>
                c.Id == topicId, "Posts").FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = _mapper.Map<BO.Topic>(topicEntity);

            return topic;
        }

        public BO.Topic GetTopic(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentNullException(nameof(topicName));

            var topicEntity = _unitOfWork.Topics.Get(c => c.Name == topicName, "").FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = _mapper.Map<BO.Topic>(topicEntity);

            return topic;
        }

        public void CreateTopic(BO.Topic topic)
        {
            if (topic is null)
                throw new ArgumentNullException(nameof(topic));

            var oldForum = GetTopic(topic.Name, topic.ForumId);

            if (oldForum != null)
                throw new DuplicateNameException("This Topic already exists under this forum.");

            var topicEntity = _mapper.Map<EO.Topic>(topic);

            _unitOfWork.Topics.Add(topicEntity);
            _unitOfWork.Save();
        }
    }
}
