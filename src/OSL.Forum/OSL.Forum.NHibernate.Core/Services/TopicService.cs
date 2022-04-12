using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using OSL.Forum.Membership.Services;
using OSL.Forum.NHibernate.Core.Enums;
using OSL.Forum.NHibernate.Core.UnitOfWorks;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;
using EO = OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Services
{
    public class TopicService : ITopicService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IProfileService _profileService;
        private IMapper _mapper;

        public TopicService(ICoreUnitOfWork unitOfWork,
            IMapper mapper, IProfileService profileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _profileService = profileService;
        }

        public BO.Topic GetTopic(string topicName, Guid forumId)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentNullException(nameof(topicName));

            var topicEntity = _unitOfWork.Topics.Get(c =>
                c.Name == topicName && c.ForumId == forumId).FirstOrDefault();

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
                c.Id == topicId).FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = _mapper.Map<BO.Topic>(topicEntity);

            return topic;
        }

        public BO.Topic GetTopic(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentNullException(nameof(topicName));

            var topicEntity = _unitOfWork.Topics.Get(c => c.Name == topicName).FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = _mapper.Map<BO.Topic>(topicEntity);

            return topic;
        }

        public void UpdateModificationDate(Guid topicId, DateTime modificationDate)
        {
            if (topicId == Guid.Empty)
                throw new ArgumentNullException(nameof(topicId));

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity == null)
                throw new ArgumentException(nameof(topicEntity));

            topicEntity.ModificationDate = modificationDate;

            _unitOfWork.Save();

        }

        public void UpdateTopicApprovalType(Guid topicId)
        {
            if (topicId == Guid.Empty)
                throw new ArgumentException(nameof(topicId));

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity == null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ApprovalType = ApprovalType.Auto.ToString();

            _unitOfWork.Save();
        }

        public int GetTopicCount(Guid forumId)
        {
            return _unitOfWork.Topics.Get(t => t.ForumId == forumId).Count;
        }

        public IList<BO.Topic> GetTopics(int pagerCurrentPage, int pagerPageSize, Guid forumId)
        {
            var topicEntities = _unitOfWork.Topics.Get(c => c.ForumId == forumId,
                q => q.OrderByDescending(c => c.ModificationDate),
                pagerCurrentPage, pagerPageSize);

            if (topicEntities == null)
                return null;

            var topics = topicEntities.Select(topic =>
                _mapper.Map<BO.Topic>(topic)
                ).ToList();

            return topics;
        }

        public void DeleteTopic(Guid topicId)
        {
            if (topicId == Guid.Empty)
                throw new ArgumentNullException(nameof(topicId));

            _unitOfWork.Topics.Remove(topicId);
            _unitOfWork.Save();
        }

        public void CloseTopic(Guid topicId)
        {
            if (topicId == Guid.Empty)
                throw new ArgumentNullException(nameof(topicId));

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity is null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ActivityStatus = ActivityStatus.Inactive.ToString();

            _unitOfWork.Save();
        }

        public void OpenTopic(Guid topicId)
        {
            if (topicId == Guid.Empty)
                throw new ArgumentNullException(nameof(topicId));

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity is null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ActivityStatus = ActivityStatus.Active.ToString();

            _unitOfWork.Save();
        }

        public void CreateTopic(BO.Topic topic)
        {
            if (topic is null)
                throw new ArgumentNullException(nameof(topic));

            var oldForum = GetTopic(topic.Name, topic.ForumId);

            if (oldForum != null)
                throw new DuplicateNameException("This Topic already exists under this forum.");

            var topicEntity = _mapper.Map<EO.Topic>(topic);
            topicEntity.Forum = _unitOfWork.Forums.GetById(topic.ForumId);
            topicEntity.ApplicationUser = _profileService.GetUser(topic.ApplicationUserId);

            _unitOfWork.Topics.Add(topicEntity);
            _unitOfWork.Save();
        }
    }
}
