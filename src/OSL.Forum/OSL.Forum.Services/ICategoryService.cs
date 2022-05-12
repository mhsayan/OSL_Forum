using System;
using System.Collections.Generic;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Services
{
    public interface ICategoryService
    {
        void CreateCategory(BO.Category category);
        BO.Category GetCategory(string categoryName);
        BO.Category GetCategory(long categoryId);
        void EditCategory(BO.Category category);
        void DeleteCategory(long categoryId);
        void UpdateModificationDate(DateTime modificationDate, long categoryId);
        long GetCategoryCount();
        IList<BO.Category> GetCategories(int pageIndex, int pageSize);
    }
}
