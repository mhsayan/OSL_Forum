using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSL.Forum.DAO;
using OSL.Forum.Entities.Contexts;
using BO = OSL.Forum.Entities.BusinessObjects;
using EO = OSL.Forum.Entities;

namespace OSL.Forum.Services
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService()
        {
            _forumRepository = new ForumRepository(new CoreDbContext());
        }

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
        }

        public virtual BO.Forum GetForum(string forumName, long categoryId)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _forumRepository.Get(forumName, categoryId);

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

        public virtual BO.Forum GetForum(long forumId)
        {
            if (forumId == 0)
                throw new ArgumentException("Forum Id is required.");

            var forumEntity = _forumRepository.GetWithIncludedProperty(forumId, "Topics");

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

        public virtual BO.Forum GetForum(string forumName)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _forumRepository.GetByName(forumName);

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

        public virtual void EditForum(BO.Forum forum)
        {
            if (forum is null)
                throw new ArgumentNullException(nameof(forum));

            var oldForum = GetForum(forum.Name);

            if (oldForum != null)
                throw new DuplicateNameException("This forum already exists.");

            var forumEntity = _forumRepository.GetById(forum.Id);

            if (forumEntity is null)
                throw new InvalidOperationException("Forum is not found.");

            forumEntity.Name = forum.Name;
            forumEntity.ModificationDate = forum.ModificationDate;
            forumEntity.ApplicationUserId = forum.ApplicationUserId;

            _forumRepository.Save();
        }

        public virtual void DeleteForum(long forumId)
        {
            if (forumId == 0)
                throw new ArgumentException("Forum id is required.");

            _forumRepository.RemoveById(forumId);
            _forumRepository.Save();
        }

        public virtual long GetForumCount(long categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category id is missing.");

            var totalForum = _forumRepository.GetCountByCategoryId(categoryId);

            return totalForum;
        }

        public virtual IList<BO.Forum> GetForums(int pagerCurrentPage, int pagerPageSize, long categoryId)
        {
            var forumEntity = _forumRepository.Load(categoryId, pagerCurrentPage, pagerPageSize, false);

            if (forumEntity == null)
                return null;

            var forums = forumEntity.Select(forum =>
                new BO.Forum()
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    CreationDate = forum.CreationDate,
                    ModificationDate = forum.ModificationDate,
                    CategoryId = forum.CategoryId,
                    ApplicationUserId = forum.ApplicationUserId
                }).ToList();

            return forums;
        }

        public virtual void CreateForum(BO.Forum forum)
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

            _forumRepository.Add(forumEntity);
            _forumRepository.Save();
        }

        public EO.Forum GetForumById(long forumId)
        {
            return _forumRepository.GetById(forumId);
        }
    }
}
