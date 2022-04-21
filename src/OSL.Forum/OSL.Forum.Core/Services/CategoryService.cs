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
    public class CategoryService : ICategoryService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CategoryService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Category GetCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentNullException(nameof(categoryName));

            var categoryEntity = _unitOfWork.Categories.Get(c => c.Name == categoryName, "").FirstOrDefault();

            if (categoryEntity == null)
                return null;

            var category = _mapper.Map<BO.Category>(categoryEntity);

            return category;
        }

        public BO.Category GetCategory(long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            var categoryEntity = _unitOfWork.Categories.GetById(categoryId);

            if (categoryEntity == null)
                return null;

            categoryEntity.Forums = categoryEntity.Forums.OrderByDescending(c => c.ModificationDate).ToList();
            var category = _mapper.Map<BO.Category>(categoryEntity);

            return category;
        }

        public IList<BO.Category> GetCategories()
        {
            var categoryEntities = _unitOfWork.Categories.Get(null, "Forums");

            var categoryList = from c in categoryEntities
                               orderby c.ModificationDate descending
                               select c;

            var categories = new List<BO.Category>();

            foreach (var entity in categoryList)
            {
                entity.Forums = entity.Forums.OrderByDescending(c => c.ModificationDate).Take(4).ToList();
                var category = _mapper.Map<BO.Category>(entity);
                categories.Add(category);
            }

            return categories;
        }

        public void EditCategory(BO.Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            var oldCategory = GetCategory(category.Name);

            if (oldCategory != null)
                throw new DuplicateNameException("This category already exists.");

            var categoryEntity = _unitOfWork.Categories.GetById(category.Id);

            if (categoryEntity is null)
                throw new InvalidOperationException("Category is not found.");

            categoryEntity.Name = category.Name;
            categoryEntity.ModificationDate = DateTime.Now;

            _unitOfWork.Save();
        }

        public void DeleteCategory(long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            _unitOfWork.Categories.Remove(categoryId);
            _unitOfWork.Save();
        }

        public void CreateCategory(BO.Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            var oldCategory = GetCategory(category.Name);

            if (oldCategory != null)
                throw new DuplicateNameException("This category name already exists.");

            category.CreationDate = DateTime.Now;
            category.ModificationDate = category.CreationDate;

            var categoryEntity = _mapper.Map<EO.Category>(category);

            _unitOfWork.Categories.Add(categoryEntity);
            _unitOfWork.Save();
        }

        public void UpdateModificationDate(DateTime modificationDate, long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            var categoryEntity = _unitOfWork.Categories.GetById(categoryId);

            if (categoryEntity is null)
                throw new InvalidOperationException("Category is not found.");

            categoryEntity.ModificationDate = modificationDate;

            _unitOfWork.Save();
        }

        public int GetCategoryCount()
        {
            return _unitOfWork.Categories.GetCount();
        }

        public IList<BO.Category> GetCategories(int pageIndex, int pageSize)
        {
            var categoryEntities = _unitOfWork.Categories.Get(null, q => q.OrderByDescending(c => c.ModificationDate), "Forums", pageIndex, pageSize, false);

            var categoryList = from c in categoryEntities.data
                               orderby c.ModificationDate descending
                               select c;

            var categories = new List<BO.Category>();

            foreach (var entity in categoryList)
            {
                entity.Forums = entity.Forums.OrderByDescending(c => c.ModificationDate).Take(4).ToList();
                var category = _mapper.Map<BO.Category>(entity);
                categories.Add(category);
            }

            return categories;
        }
    }
}
