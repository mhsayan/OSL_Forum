using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Repositories;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService()
        {
            _topicRepository = new TopicRepository();
        }

        public BO.Topic GetTopic(string topicName, long forumId)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentNullException(nameof(topicName));

            var topicEntity = _topicRepository.Get(topicName, forumId);

            if (topicEntity == null)
                return null;

            var topic = new BO.Topic
            {
                Id = topicEntity.Id,
                Name = topicEntity.Name,
                ForumId = topicEntity.ForumId,
                Forum = new BO.Forum()
                {
                    Id = topicEntity.Forum.Id,
                    Name = topicEntity.Forum.Name,
                    Category = new BO.Category()
                    {
                        Id = topicEntity.Forum.Category.Id,
                        Name = topicEntity.Forum.Category.Name
                    }
                },
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

            var topicEntity = _topicRepository.GetWithIncludedProperty(topicId, "Posts");

            if (topicEntity == null)
                return null;

            var topic = new BO.Topic
            {
                Id = topicEntity.Id,
                Name = topicEntity.Name,
                ForumId = topicEntity.ForumId,
                Forum = new BO.Forum()
                {
                    Id = topicEntity.Forum.Id,
                    Name = topicEntity.Forum.Name,
                    Category = new BO.Category()
                    {
                        Id = topicEntity.Forum.Category.Id,
                        Name = topicEntity.Forum.Category.Name
                    }
                },
                CreationDate = topicEntity.CreationDate,
                ModificationDate = topicEntity.ModificationDate,
                ActivityStatus = topicEntity.ActivityStatus,
                ApprovalType = topicEntity.ApprovalType,
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

            var topicEntity = _topicRepository.GetByName(topicName);

            if (topicEntity == null)
                return null;

            var topic = new BO.Topic
            {
                Id = topicEntity.Id,
                Name = topicEntity.Name,
                ForumId = topicEntity.ForumId,
                Forum = new BO.Forum()
                {
                    Id = topicEntity.ForumId
                },
                CreationDate = topicEntity.CreationDate,
                ModificationDate = topicEntity.ModificationDate
            };

            return topic;
        }

        public void UpdateModificationDate(long topicId, DateTime modificationDate)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _topicRepository.GetById(topicId);

            if (topicEntity == null)
                throw new ArgumentException(nameof(topicEntity));

            topicEntity.ModificationDate = modificationDate;

            _topicRepository.Save();

        }

        public void UpdateTopicApprovalType(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _topicRepository.GetById(topicId);

            if (topicEntity == null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ApprovalType = ApprovalType.Auto.ToString();

            _topicRepository.Save();
        }

        public long GetTopicCount(long forumId)
        {
            return _topicRepository.GetCountByForumId(forumId);
        }

        public IList<BO.Topic> GetTopics(int pagerCurrentPage, int pagerPageSize, long forumId)
        {
            var topicEntities = _topicRepository.Load(forumId, pagerCurrentPage, pagerPageSize, false);

            var topics = topicEntities?.Select(topicEntity =>
                new BO.Topic()
                {
                    Id = topicEntity.Id,
                    Name = topicEntity.Name,
                    ForumId = topicEntity.ForumId,
                    Forum = new BO.Forum()
                    {
                        Id = topicEntity.Forum.Id,
                        Name = topicEntity.Forum.Name,
                        Category = new BO.Category()
                        {
                            Id = topicEntity.Forum.Category.Id,
                            Name = topicEntity.Forum.Category.Name
                        }
                    },
                    CreationDate = topicEntity.CreationDate,
                    ModificationDate = topicEntity.ModificationDate,
                    ApplicationUserId = topicEntity.ApplicationUserId,
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
                }).ToList();

            return topics;
        }

        public void DeleteTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            _topicRepository.RemoveById(topicId);
            _topicRepository.Save();
        }

        public void CloseTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _topicRepository.GetById(topicId);

            if (topicEntity is null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ActivityStatus = ActivityStatus.Inactive.ToString();

            _topicRepository.Save();
        }

        public void OpenTopic(long topicId)
        {
            if (topicId == 0)
                throw new ArgumentException("Topic Id is required.");

            var topicEntity = _topicRepository.GetById(topicId);

            if (topicEntity is null)
                throw new InvalidOperationException("Topic is not found.");

            topicEntity.ActivityStatus = ActivityStatus.Active.ToString();

            _topicRepository.Save();
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

            _topicRepository.Add(topicEntity);
            _topicRepository.Save();
        }
    }
}
