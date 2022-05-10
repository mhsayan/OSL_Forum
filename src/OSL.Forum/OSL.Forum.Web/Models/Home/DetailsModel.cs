using System.Collections.Generic;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Home
{
    public class DetailsModel : BaseModel
    {
        public BO.Category Category { get; set; }
        public IList<string> Roles { get; set; }
        public IList<BO.Forum> Forums { get; set; }
        public Pager Pager { get; set; }
        private ICategoryService _categoryService;
        private IForumService _forumService;
        private IProfileService _profileService;

        public DetailsModel()
        {
        }

        public override async Task Resolve()
        {
            _categoryService = new CategoryService();
            _forumService = new ForumService();
            _profileService = new ProfileService();

            await base.Resolve();
        }

        public void GetCategory(long categoryId)
        {
            Category = _categoryService.GetCategory(categoryId);
        }

        public void GetForums(long categoryId, int? page)
        {
            var totalItem = _forumService.GetForumCount(categoryId);

            Pager = new Pager(totalItem, page);
            Forums = _forumService.GetForums(Pager.CurrentPage, Pager.PageSize, categoryId);
        }

        public async Task LoadUserInfo()
        {
            Roles = await _profileService.UserRolesAsync();
        }
    }
}