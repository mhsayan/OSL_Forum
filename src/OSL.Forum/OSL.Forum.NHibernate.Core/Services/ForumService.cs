using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using OSL.Forum.NHibernate.Core.UnitOfWorks;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;
using EO = OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Services
{
    public class ForumService : IForumService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ForumService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Forum GetForum(string forumName, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName && c.CategoryId == categoryId).FirstOrDefault();

            if (forumEntity == null)
                return null;

            var forum = _mapper.Map<BO.Forum>(forumEntity);

            return forum;
        }

        public BO.Forum GetForum(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Id == forumId).FirstOrDefault();

            if (forumEntity == null)
                return null;

            forumEntity.Topics = forumEntity.Topics.OrderByDescending(t => t.ModificationDate).ToList();
            var forum = _mapper.Map<BO.Forum>(forumEntity);

            return forum;
        }

        public BO.Forum GetForum(string forumName)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName).FirstOrDefault();

            if (forumEntity == null)
                return null;

            var forum = _mapper.Map<BO.Forum>(forumEntity);

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

        public void DeleteForum(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            _unitOfWork.Forums.Remove(forumId);
            _unitOfWork.Save();
        }

        public int GetForumCount(Guid categoryId)
        {
            var totalForum = _unitOfWork.Forums.Get(f => f.CategoryId == categoryId).Count;

            return totalForum;
        }

        public IList<BO.Forum> GetForums(int pagerCurrentPage, int pagerPageSize, Guid categoryId)
        {
            var forumEntities = _unitOfWork.Forums.Get(c => c.CategoryId == categoryId, q => q.OrderByDescending(c => c.ModificationDate), pagerCurrentPage, pagerPageSize);

            if (forumEntities == null)
                return null;

            var forums = forumEntities.Select(forum =>
                _mapper.Map<BO.Forum>(forum)
                ).ToList();

            return forums;
        }

        public void CreateForum(BO.Forum forum)
        {
            if (forum is null)
                throw new ArgumentNullException(nameof(forum));

            var oldForum = GetForum(forum.Name, forum.CategoryId);

            if (oldForum != null)
                throw new DuplicateNameException("This Forum name already exists under this category.");

            var forumEntity = _mapper.Map<EO.Forum>(forum);

            _unitOfWork.Forums.Add(forumEntity);
            _unitOfWork.Save();
        }
    }
}
