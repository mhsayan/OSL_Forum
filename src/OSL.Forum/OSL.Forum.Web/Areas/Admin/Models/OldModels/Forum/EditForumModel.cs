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
    public class EditForumModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public BO.Forum BoForum { get; set; }
        private IForumService _forumService;
        private ICategoryService _categoryService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;

        public EditForumModel()
        {
            _categoryService = new CategoryService();
            _forumService = new ForumService();
            _dateTimeUtility = new DateTimeUtility();
            _profileService = new ProfileService();
        }

        public void GetForum(long forumId)
        {
            if (forumId == 0)
                throw new ArgumentException(nameof(forumId));

            BoForum = _forumService.GetForum(forumId);

            if (BoForum == null)
                throw new InvalidOperationException("Forum not found");

            this.Id = BoForum.Id;
            this.Name = BoForum.Name;
            this.CategoryId = BoForum.CategoryId;
        }

        public void Edit()
        {
            var user = _profileService.GetUser();
            var modificationDate = _dateTimeUtility.Now;

            var forum = new BO.Forum
            {
                Id = this.Id,
                Name = this.Name,
                CategoryId = this.CategoryId,
                ModificationDate = modificationDate,
                ApplicationUserId = user.Id
            };

            _forumService.EditForum(forum);
            _categoryService.UpdateModificationDate(modificationDate, forum.CategoryId);
        }

        public void Delete(long forumId)
        {
            if (forumId == 0)
                throw new ArgumentException("Forum Id is required");

            _forumService.DeleteForum(forumId);
        }
    }
}