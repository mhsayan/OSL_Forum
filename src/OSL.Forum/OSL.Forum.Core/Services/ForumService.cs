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

            var forumEntity = _unitOfWork.Forums.GetById(forumId);

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

        #region Helper Region

        //public BO.Category GetCategory(Guid categoryId)
        //{
        //    if (categoryId == Guid.Empty)
        //        throw new ArgumentNullException(nameof(categoryId));

        //    var categoryEntity = _unitOfWork.Categories.GetById(categoryId);

        //    if (categoryEntity == null)
        //        return null;

        //    var category = _mapper.Map<BO.Category>(categoryEntity);

        //    return category;
        //}

        //public IList<BO.Category> GetCategories()
        //{
        //    var categoryEntities = _unitOfWork.Categories.GetAll();
        //    var categoryList = from c in categoryEntities
        //                       orderby c.ModificationDate descending
        //                       select c;

        //    var categories = new List<BO.Category>();

        //    foreach (var entity in categoryList)
        //    {
        //        var category = _mapper.Map<BO.Category>(entity);
        //        categories.Add(category);
        //    }

        //    return categories;
        //}

        //public void DeleteCategory(Guid categoryId)
        //{
        //    if (categoryId == Guid.Empty)
        //        throw new ArgumentNullException(nameof(categoryId));

        //    _unitOfWork.Categories.Remove(categoryId);
        //    _unitOfWork.Save();
        //}

        #endregion

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
