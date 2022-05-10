using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Forum
{
    public class CreateForumModel
    {
        [Required]
        public long CategoryId { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public BO.Category BoCategory { get; set; }
        private IProfileService _profileService;
        private IForumService _forumService;
        private IDateTimeUtility _dateTimeUtility;
        private ICategoryService _categoryService;

        public CreateForumModel()
        {
            _profileService = new ProfileService();
            _dateTimeUtility = new DateTimeUtility();
            _forumService = new ForumService();
            _categoryService = new CategoryService();
        }

        public void GetCategory(long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            BoCategory = _categoryService.GetCategory(categoryId);

            if (BoCategory == null)
                throw new InvalidOperationException("Forum not found");
        }

        public void Create()
        {
            var user = _profileService.GetUser();

            if (user == null)
                throw new InvalidOperationException(nameof(user));

            var time = _dateTimeUtility.Now;
            var forum = new BO.Forum()
            {
                Name = this.Name,
                CategoryId = this.CategoryId,
                ApplicationUserId = user.Id,
                CreationDate = time,
                ModificationDate = time
            };

            _forumService.CreateForum(forum);
            _categoryService.UpdateModificationDate(time, forum.CategoryId);
        }
    }
}