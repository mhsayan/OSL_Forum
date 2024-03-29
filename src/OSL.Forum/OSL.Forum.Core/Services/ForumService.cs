﻿using System;
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
    public class ForumService : IForumService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ForumService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Forum GetForum(string forumName, long categoryId)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName && c.CategoryId == categoryId, "").FirstOrDefault();

            if (forumEntity == null)
                return null;

            var forum = _mapper.Map<BO.Forum>(forumEntity);

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
            var forum = _mapper.Map<BO.Forum>(forumEntity);

            return forum;
        }

        public BO.Forum GetForum(string forumName)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName, "").FirstOrDefault();

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

            var forumList = from c in data
                            orderby c.ModificationDate descending
                            select c;

            var forums = forumList.Select(forum =>
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
