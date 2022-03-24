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
    public class PostService : IPostService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public PostService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Forum GetForum(string forumName, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(forumName))
                throw new ArgumentNullException(nameof(forumName));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Name == forumName && c.CategoryId == categoryId, "").FirstOrDefault();

            if (forumEntity == null)
                return null;

            var forum = _mapper.Map<BO.Forum>(forumEntity);

            return forum;
        }

        public BO.Forum GetForum(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            var forumEntity = _unitOfWork.Forums.Get(c => c.Id == forumId, "Topics").FirstOrDefault();

            if (forumEntity == null)
                return null;

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

            _unitOfWork.Save();
        }

        public void DeleteForum(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            _unitOfWork.Forums.Remove(forumId);
            _unitOfWork.Save();
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
