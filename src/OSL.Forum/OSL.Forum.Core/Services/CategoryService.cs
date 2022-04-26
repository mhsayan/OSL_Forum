using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private static CategoryService _categoryService;
        private readonly CoreUnitOfWork _unitOfWork;

        private CategoryService()
        {
            _unitOfWork = CoreUnitOfWork.Create();
        }

        public static CategoryService Create()
        {
            if (_categoryService == null)
            {
                _categoryService = new CategoryService();
            }

            return new CategoryService();
        }

        public BO.Category GetCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentNullException(nameof(categoryName));

            var categoryEntity = _unitOfWork.Categories.Get(c => c.Name == categoryName, "").FirstOrDefault();

            if (categoryEntity == null)
                return null;

            var category = new BO.Category()
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                CreationDate = categoryEntity.CreationDate,
                ModificationDate = categoryEntity.ModificationDate,
                Forums = categoryEntity.Forums.Select(f => new BO.Forum()
                {
                    Id = f.Id,
                    Name = f.Name,
                    CreationDate = f.CreationDate,
                    ModificationDate = f.ModificationDate,
                    CategoryId = f.CategoryId,
                    Category = new BO.Category()
                    {
                        Id = f.Category.Id,
                        Name = f.Category.Name,
                        CreationDate = f.Category.CreationDate,
                        ModificationDate = f.Category.ModificationDate
                    }
                }).ToList()

            };

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

            var category = new BO.Category
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                CreationDate = categoryEntity.CreationDate,
                ModificationDate = categoryEntity.ModificationDate,
                Forums = categoryEntity.Forums.Select(forum => new BO.Forum()
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    CreationDate = forum.CreationDate,
                    ModificationDate = forum.ModificationDate,
                    CategoryId = forum.CategoryId,
                    Category = new BO.Category()
                    {
                        Id = forum.Category.Id,
                        Name = forum.Category.Name,
                        CreationDate = forum.Category.CreationDate,
                        ModificationDate = forum.Category.ModificationDate
                    }
                }).ToList()
            };

            return category;
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

            var categoryEntity = new EO.Category()
            {
                Name = category.Name,
                CreationDate = category.CreationDate,
                ModificationDate = category.ModificationDate
            };

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

            var categories = new List<BO.Category>();

            foreach (var entity in categoryEntities.data)
            {
                entity.Forums = entity.Forums.OrderByDescending(c => c.ModificationDate).Take(4).ToList();

                var category = new BO.Category()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CreationDate = entity.CreationDate,
                    ModificationDate = entity.ModificationDate,
                    Forums = entity.Forums.Select(forum => new BO.Forum()
                    {
                        Id = forum.Id,
                        Name = forum.Name,
                        CreationDate = forum.CreationDate,
                        ModificationDate = forum.ModificationDate,
                        CategoryId = forum.CategoryId,
                        Category = new BO.Category()
                        {
                            Id = forum.Category.Id,
                            Name = forum.Category.Name,
                            CreationDate = forum.Category.CreationDate,
                            ModificationDate = forum.Category.ModificationDate
                        }
                    }).ToList()
                };

                categories.Add(category);
            }

            return categories;
        }
    }
}
