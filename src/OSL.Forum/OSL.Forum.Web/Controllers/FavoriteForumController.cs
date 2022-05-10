using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Web.Models.FavoriteForum;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class FavoriteForumController : Controller
    {
        private readonly ILog _logger;

        public FavoriteForumController()
        {
            _logger = LogManager.GetLogger(typeof(FavoriteForumController));
        }

        public ActionResult FavoriteForums(int? page)
        {
            var model = new FavoriteForumModel();
            model.GetFavoriteForums(page);

            return View(model);
        }

        public ActionResult AddToFavorite(long forumId)
        {
            var model = new FavoriteForumModel();
            model.AddToFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        public ActionResult RemoveFromFavorite(long forumId)
        {
            var model = new FavoriteForumModel();
            model.RemoveFromFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}