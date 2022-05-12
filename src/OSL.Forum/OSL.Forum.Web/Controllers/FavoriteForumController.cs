using log4net;
using OSL.Forum.Common.Utilities;
using OSL.Forum.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;
using System.Web.Mvc;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class FavoriteForumController : Controller
    {
        private readonly ILog _logger;
        private readonly IFavoriteForumService _favoriteForumService;
        private readonly IProfileService _profileService;

        public FavoriteForumController()
        {
            _logger = LogManager.GetLogger(typeof(FavoriteForumController));
            _favoriteForumService = new FavoriteForumService();
            _profileService = new ProfileService();
        }

        public ActionResult FavoriteForums(int? page)
        {
            var model = new FavoriteForumModel();

            var user = _profileService.GetUser();
            var totalItem = _favoriteForumService.GetFavoriteForumCount(user.Id);

            model.Pager = new Pager(totalItem, page);
            model.FavoriteForums = _favoriteForumService.GetUserFavoriteForums(
                model.Pager.CurrentPage, model.Pager.PageSize, user.Id);

            return View(model);
        }

        public ActionResult AddToFavorite(long forumId)
        {
            var model = new FavoriteForumModel();
            var user = _profileService.GetUser();

            _favoriteForumService.AddToFavorite(forumId, user.Id);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        public ActionResult RemoveFromFavorite(long forumId)
        {
            var user = _profileService.GetUser();

            _favoriteForumService.RemoveFromFavorite(forumId, user.Id);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}