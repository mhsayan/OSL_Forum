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
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public EditCategoryModel()
        {
        }

        public EditCategoryModel(ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();

            await base.ResolveAsync(_scope);
        }

        public void GetCategory(long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            var category = _categoryService.GetCategory(categoryId);

            if (category == null)
                throw new InvalidOperationException("Category not found");

            _mapper.Map(category, this);
        }

        public void Edit()
        {
            var category = _mapper.Map<BO.Category>(this);

            _categoryService.EditCategory(category);
        }
    }
}