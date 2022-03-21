using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
{
    public interface ICategoryService
    {
        void CreateCategory(BO.Category category);
        BO.Category GetCategory(string categoryName);
        BO.Category GetCategory(Guid categoryId);
        IList<BO.Category> GetCategories();
        void EditCategory(BO.Category category);
        void DeleteCategory(Guid categoryId);
    }
}
