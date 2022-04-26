using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class ForumService : IForumService
    {
        private static ForumService _forumService;
        private readonly CoreUnitOfWork _unitOfWork;

        private ForumService()
        {
            _unitOfWork = CoreUnitOfWork.Create();
        }

        public static ForumService Create()
        {
            if (_forumService == null)
            {
                _forumService = new ForumService();
            }

            return _forumService;
        }

        public BO.Forum GetForum(string forumName, long categoryId)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName && c.CategoryId == categoryId, "").FirstOrDefault();

            if (forumEntity == null)
                return null;

            var forum = new BO.Forum()
            {
                Id = forumEntity.Id,
                Name = forumEntity.Name,
                CreationDate = forumEntity.CreationDate,
                ModificationDate = forumEntity.ModificationDate,
                CategoryId = forumEntity.CategoryId,
                ApplicationUserId = forumEntity.ApplicationUserId,
                Category = new BO.Category()
                {
                    Id = forumEntity.Category.Id,
                    Name = forumEntity.Category.Name,
                    CreationDate = forumEntity.Category.CreationDate,
                    ModificationDate = forumEntity.Category.ModificationDate
                },
                Topics = forumEntity.Topics.Select(topic => new BO.Topic()
                {
                    Id = topic.Id,
                    Name = topic.Name,
                    CreationDate = topic.CreationDate,
                    ModificationDate = topic.ModificationDate,
                    ApplicationUserId = topic.ApplicationUserId,
                    ForumId = topic.ForumId,
                    Posts = topic.Posts.Select(post => new BO.Post()
                    {
                        Id = post.Id,
                        Name = post.Name,
                        CreationDate = post.CreationDate,
                        ModificationDate = post.ModificationDate,
                        ApplicationUserId = post.ApplicationUserId,
                        TopicId = post.TopicId
                    }).ToList()
                }).ToList()
            };

            return forum;
        }

        public BO.Forum GetForum(long forumId)
        {
            if (forumId == 0)
                throw new ArgumentException("Forum Id is required.");

            var forumEntity = _unitOfWork.Forums.Get(c => c.Id == forumId, "Topics").FirstOrDefault();

            if (forumEntity == null)
                return null;

            forumEntity.Topics = forumEntity.Topics.OrderByDescending(t => t.ModificationDate).ToList();

            var forum = new BO.Forum()
            {
                Id = forumEntity.Id,
                Name = forumEntity.Name,
                CreationDate = forumEntity.CreationDate,
                ModificationDate = forumEntity.ModificationDate,
                CategoryId = forumEntity.CategoryId,
                ApplicationUserId = forumEntity.ApplicationUserId,
                Category = new BO.Category()
                {
                    Id = forumEntity.Category.Id,
                    Name = forumEntity.Category.Name,
                    CreationDate = forumEntity.Category.CreationDate,
                    ModificationDate = forumEntity.Category.ModificationDate
                },
                Topics = forumEntity.Topics.Select(topic => new BO.Topic()
                {
                    Id = topic.Id,
                    Name = topic.Name,
                    CreationDate = topic.CreationDate,
                    ModificationDate = topic.ModificationDate,
                    ApplicationUserId = topic.ApplicationUserId,
                    ForumId = topic.ForumId,
                    Posts = topic.Posts.Select(post => new BO.Post()
                    {
                        Id = post.Id,
                        Name = post.Name,
                        CreationDate = post.CreationDate,
                        ModificationDate = post.ModificationDate,
                        ApplicationUserId = post.ApplicationUserId,
                        TopicId = post.TopicId
                    }).ToList()
                }).ToList()
            };

            return forum;
        }

        public BO.Forum GetForum(string forumName)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName, "").FirstOrDefault();

            if (forumEntity == null)
                return null;

            var forum = new BO.Forum()
            {
                Id = forumEntity.Id,
                Name = forumEntity.Name,
                CreationDate = forumEntity.CreationDate,
                ModificationDate = forumEntity.ModificationDate,
                CategoryId = forumEntity.CategoryId,
                ApplicationUserId = forumEntity.ApplicationUserId,
                Category = new BO.Category()
                {
                    Id = forumEntity.Category.Id,
                    Name = forumEntity.Category.Name,
                    CreationDate = forumEntity.Category.CreationDate,
                    ModificationDate = forumEntity.Category.ModificationDate
                },
                Topics = forumEntity.Topics.Select(topic => new BO.Topic()
                {
                    Id = topic.Id,
                    Name = topic.Name,
                    CreationDate = topic.CreationDate,
                    ModificationDate = topic.ModificationDate,
                    ApplicationUserId = topic.ApplicationUserId,
                    ForumId = topic.ForumId,
                    Posts = topic.Posts.Select(post => new BO.Post()
                    {
                        Id = post.Id,
                        Name = post.Name,
                        CreationDate = post.CreationDate,
                        ModificationDate = post.ModificationDate,
                        ApplicationUserId = post.ApplicationUserId,
                        TopicId = post.TopicId
                    }).ToList()
                }).ToList()
            };

            return forum;
        }

        public void EditForum(BO.Forum forum)
        {
            if (forum is null)
                throw new ArgumentNullException(nameof(forum));

            var oldForum = GetForum(forum.Name);

            if (oldForum != null)
                throw new DuplicateNameException("This forum already exists.");

            var forumEntity = _unitOfWork.Forums.GetById(forum.Id);

            if (forumEntity is null)
                throw new InvalidOperationException("Forum is not found.");

            forumEntity.Name = forum.Name;
            forumEntity.ModificationDate = forum.ModificationDate;
            forumEntity.ApplicationUserId = forum.ApplicationUserId;

            _unitOfWork.Save();
        }

        public void DeleteForum(long forumId)
        {
            if (forumId == 0)
                throw new ArgumentException("Forum id is required.");

            _unitOfWork.Forums.Remove(forumId);
            _unitOfWork.Save();
        }

        public int GetForumCount(long categoryId)
        {
            var totalForum = _unitOfWork.Forums.Get(f => f.CategoryId == categoryId, "").Count;

            return totalForum;
        }

        public IList<BO.Forum> GetForums(int pagerCurrentPage, int pagerPageSize, long categoryId)
        {
            var (data, _, _) = _unitOfWork.Forums.Get(c => c.CategoryId == categoryId, q => q.OrderByDescending(c => c.ModificationDate), "", pagerCurrentPage, pagerPageSize, false);

            if (data == null)
                return null;

            var forums = data.Select(forum =>
                new BO.Forum()
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    CreationDate = forum.CreationDate,
                    ModificationDate = forum.ModificationDate,
                    CategoryId = forum.CategoryId,
                    ApplicationUserId = forum.ApplicationUserId,
                    Category = new BO.Category()
                    {
                        Id = forum.Category.Id,
                        Name = forum.Category.Name,
                        CreationDate = forum.Category.CreationDate,
                        ModificationDate = forum.Category.ModificationDate
                    },
                    Topics = forum.Topics.Select(topic => new BO.Topic()
                    {
                        Id = topic.Id,
                        Name = topic.Name,
                        CreationDate = topic.CreationDate,
                        ModificationDate = topic.ModificationDate,
                        ApplicationUserId = topic.ApplicationUserId,
                        ForumId = topic.ForumId,
                    }).ToList()
                }).ToList();

            return forums;
        }

        public void CreateForum(BO.Forum forum)
        {
            if (forum is null)
                throw new ArgumentNullException(nameof(forum));

            var oldForum = GetForum(forum.Name, forum.CategoryId);

            if (oldForum != null)
                throw new DuplicateNameException("This Forum name already exists under this category.");

            var forumEntity = new EO.Forum()
            {
                Name = forum.Name,
                CategoryId = forum.CategoryId,
                ApplicationUserId = forum.ApplicationUserId,
                CreationDate = forum.CreationDate,
                ModificationDate = forum.ModificationDate
            };

            _unitOfWork.Forums.Add(forumEntity);
            _unitOfWork.Save();
        }
    }
}
