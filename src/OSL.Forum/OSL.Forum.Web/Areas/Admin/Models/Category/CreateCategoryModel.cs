using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CreateCategoryModel : BaseModel
    {
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CreateCategoryModel()
        {
        }

        protected override Task Resolve()
        {
            _categoryService = CategoryService.Create();

            return Task.CompletedTask;
        }

        public void Create()
        {
            var category = _mapper.Map<BO.Category>(this);

            _categoryService.CreateCategory(category);
        }
    }
}