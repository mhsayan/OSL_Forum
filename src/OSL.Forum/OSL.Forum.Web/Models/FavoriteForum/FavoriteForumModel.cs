using System.Collections.Generic;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.FavoriteForum
{
    public class FavoriteForumModel : BaseModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public Pager Pager { get; set; }
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;

        public FavoriteForumModel()
        {

        }

        public override async Task Resolve()
        {
            _favoriteForumService = new FavoriteForumService();
            _profileService = new ProfileService();

            await base.Resolve();
        }

        public void AddToFavorite(long forumId)
        {
            var user = _profileService.GetUser();

            _favoriteForumService.AddToFavorite(forumId, user.Id);
        }

        public void RemoveFromFavorite(long forumId)
        {
            var user = _profileService.GetUser();

            _favoriteForumService.RemoveFromFavorite(forumId, user.Id);
        }

        public void GetFavoriteForums(int? page)
        {
            var user = _profileService.GetUser();
            var totalItem = _favoriteForumService.GetFavoriteForumCount(user.Id);

            Pager = new Pager(totalItem, page);
            FavoriteForums = _favoriteForumService.GetUserFavoriteForums(Pager.CurrentPage, Pager.PageSize, user.Id);
        }
    }
}