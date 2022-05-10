using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Home
{
    public class IndexViewModel : BaseModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public IList<BO.Category> Categories { get; set; }
        public bool IsAuthenticated { get; set; }
        public Pager Pager { get; set; }
        private ICategoryService _categoryService;
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;

        public IndexViewModel()
        {
        }

        public override async Task Resolve()
        {
            _categoryService = new CategoryService();
            _favoriteForumService = new FavoriteForumService();
            _profileService = new ProfileService();

            await base.Resolve();
        }

        public void GetCategories(int? page)
        {
            var totalItem = _categoryService.GetCategoryCount();

            Pager = new Pager(totalItem, page);

            Categories = _categoryService.GetCategories(Pager.CurrentPage, Pager.PageSize);
        }

        public void UserAuthenticatedStatus()
        {
            IsAuthenticated = _profileService.IsAuthenticated();
        }

        public void GetFavoriteForums()
        {
            var user = _profileService.GetUser();
            FavoriteForums = _favoriteForumService.GetUserFavoriteForums(user.Id).Take(4).ToList();
        }
    }
}