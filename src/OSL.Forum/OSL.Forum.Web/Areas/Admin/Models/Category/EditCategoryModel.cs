using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class EditCategoryModel : BaseModel
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
        }

        public override Task Resolve()
        {
            _categoryService = CategoryService.Create();

            return Task.CompletedTask;
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