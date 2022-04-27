using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSL.Forum.Core.Enums;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class TopicService : ITopicService
    {
        private readonly CoreUnitOfWork _unitOfWork;

        private TopicService()
        {
            _unitOfWork = CoreUnitOfWork.CreateTopicRepository();
        }

        public static TopicService Create()
        {
            return new TopicService();
        }

        public BO.Topic GetTopic(string topicName, long forumId)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentNullException(nameof(topicName));

            var topicEntity = _unitOfWork.Topics.Get(c =>
                c.Name == topicName && c.ForumId == forumId, "").FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = new BO.Topic
            {
                Id = topicEntity.Id,
                Name = topicEntity.Name,
                ForumId = topicEntity.ForumId,
                Forum = ForumService.Create().GetForum(topicEntity.ForumId),
                CreationDate = topicEntity.CreationDate,
                ModificationDate = topicEntity.ModificationDate,
                Posts = topicEntity.Posts.Select(post => new BO.Post
                {
                    Id = post.Id,
                    Name = post.Name,
                    TopicId = post.TopicId,
                    CreationDate = post.CreationDate,
                    ModificationDate = post.ModificationDate,
                    ApplicationUserId = post.ApplicationUserId,
                    Description = post.Description,
                    Status = post.Status,
                }).ToList()
            };

            return topic;
        }

        public BO.Topic GetTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _unitOfWork.Topics.Get(c =>
                c.Id == topicId, "Posts").FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = new BO.Topic
            {
                Id = topicEntity.Id,
                Name = topicEntity.Name,
                ForumId = topicEntity.ForumId,
                Forum = ForumService.Create().GetForum(topicEntity.ForumId),
                CreationDate = topicEntity.CreationDate,
                ModificationDate = topicEntity.ModificationDate,
                ActivityStatus = topicEntity.ActivityStatus,
                Posts = topicEntity.Posts.Select(post => new BO.Post
                {
                    Id = post.Id,
                    Name = post.Name,
                    TopicId = post.TopicId,
                    CreationDate = post.CreationDate,
                    ModificationDate = post.ModificationDate,
                    ApplicationUserId = post.ApplicationUserId,
                    Description = post.Description,
                    Status = post.Status,
                }).ToList()
            };

            return topic;
        }

        public BO.Topic GetTopic(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException(nameof(topicName));

            var topicEntity = _unitOfWork.Topics.Get(c => c.Name == topicName, "").FirstOrDefault();

            if (topicEntity == null)
                return null;

            var topic = new BO.Topic
            {
                Id = topicEntity.Id,
                Name = topicEntity.Name,
                ForumId = topicEntity.ForumId,
                Forum = ForumService.Create().GetForum(topicEntity.ForumId),
                CreationDate = topicEntity.CreationDate,
                ModificationDate = topicEntity.ModificationDate
            };

            return topic;
        }

        public void UpdateModificationDate(long topicId, DateTime modificationDate)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity == null)
                throw new ArgumentException(nameof(topicEntity));

            topicEntity.ModificationDate = modificationDate;

            _unitOfWork.Save();

        }

        public void UpdateTopicApprovalType(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity == null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ApprovalType = ApprovalType.Auto.ToString();

            _unitOfWork.Save();
        }

        public int GetTopicCount(long forumId)
        {
            return _unitOfWork.Topics.Get(t => t.ForumId == forumId, "").Count;
        }

        public IList<BO.Topic> GetTopics(int pagerCurrentPage, int pagerPageSize, long forumId)
        {
            var topicEntities = _unitOfWork.Topics.Get(c => c.ForumId == forumId,
                q => q.OrderByDescending(c => c.ModificationDate), "",
                pagerCurrentPage, pagerPageSize, false);

            if (topicEntities.data == null)
                return null;

            var topics = topicEntities.data.Select(topic =>
                new BO.Topic()
                {
                    Id = topic.Id,
                    Name = topic.Name,
                    ForumId = topic.ForumId,
                    Forum = ForumService.Create().GetForum(topic.ForumId),
                    CreationDate = topic.CreationDate,
                    ModificationDate = topic.ModificationDate,
                    ApplicationUserId = topic.ApplicationUserId,
                    ActivityStatus = topic.ActivityStatus,
                    Posts = topic.Posts.Select(post => new BO.Post
                    {
                        Id = post.Id,
                        Name = post.Name,
                        TopicId = post.TopicId,
                        CreationDate = post.CreationDate,
                        ModificationDate = post.ModificationDate,
                        ApplicationUserId = post.ApplicationUserId,
                        Description = post.Description,
                        Status = post.Status,
                    }).ToList()
                }).ToList();

            return topics;
        }

        public void DeleteTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            _unitOfWork.Topics.Remove(topicId);
            _unitOfWork.Save();
        }

        public void CloseTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _unitOfWork.Topics.GetById(topicId);

            if (topicEntity is null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ActivityStatus = ActivityStatus.Inactive.ToString();

            _unitOfWork.Save();
        }

        public void OpenTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

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

            var topicEntity = new EO.Topic
            {
                Name = topic.Name,
                ForumId = topic.ForumId,
                ApplicationUserId = topic.ApplicationUserId,
                Status = topic.Status,
                CreationDate = topic.CreationDate,
                ModificationDate = topic.ModificationDate,
                ActivityStatus = topic.ActivityStatus,
                ApprovalType = topic.ApprovalType
            };

            _unitOfWork.Topics.Add(topicEntity);
            _unitOfWork.Save();
        }
    }
}
