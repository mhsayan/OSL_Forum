using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
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

        public async Task<ActionResult> FavoriteForums(int? page)
        {
            var model = new FavoriteForumModel();
            await model.Resolve();
            model.GetFavoriteForums(page);

            return View(model);
        }

        public async Task<ActionResult> AddToFavorite(long forumId)
        {
            var model = new FavoriteForumModel();
            await model.Resolve();
            model.AddToFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        public async Task<ActionResult> RemoveFromFavorite(long forumId)
        {
            var model = new FavoriteForumModel();
            await model.Resolve();
            model.RemoveFromFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}