using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class EditCategoryModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private ICategoryService _categoryService;

        public EditCategoryModel()
        {
            _categoryService = new CategoryService();
        }

        public void GetCategory(long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            var category = _categoryService.GetCategory(categoryId);

            if (category == null)
                throw new InvalidOperationException("Category not found");

            this.Id = category.Id;
            this.Name = category.Name;
        }

        public void Edit()
        {
            var category = new BO.Category()
            {
                Id = this.Id,
                Name = this.Name
            };

            _categoryService.EditCategory(category);
        }
    }
}