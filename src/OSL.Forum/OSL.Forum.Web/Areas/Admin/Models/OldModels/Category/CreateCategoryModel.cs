using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CreateCategoryModel
    {
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private ICategoryService _categoryService;
        private IDateTimeUtility _dateTimeUtility;

        public CreateCategoryModel()
        {
            _categoryService = new CategoryService();
            _dateTimeUtility = new DateTimeUtility();
        }

        public void Create()
        {
            var time = _dateTimeUtility.Now;

            var category = new BO.Category()
            {
                Name = this.Name,
                CreationDate = time,
                ModificationDate = time
            };


            _categoryService.CreateCategory(category);
        }
    }
}