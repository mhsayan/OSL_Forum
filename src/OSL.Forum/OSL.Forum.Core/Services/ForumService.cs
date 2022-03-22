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

        //public void EditCategory(BO.Category category)
        //{
        //    if (category is null)
        //        throw new ArgumentNullException(nameof(category));

        //    var oldCategory = GetCategory(category.Name);

        //    if (oldCategory != null)
        //        throw new DuplicateNameException("This category already exists.");

        //    var categoryEntity = _unitOfWork.Categories.GetById(category.Id);

        //    if (categoryEntity is null)
        //        throw new InvalidOperationException("Category is not found.");

        //    categoryEntity.Name = category.Name;
        //    categoryEntity.ModificationDate = DateTime.Now;

        //    _unitOfWork.Save();
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
