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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(FavoriteForumController));
        private readonly ILifetimeScope _scope;

        public FavoriteForumController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task<ActionResult> FavoriteForums()
        {
            var model = _scope.Resolve<FavoriteForumModel>();
            await model.ResolveAsync(_scope);
            model.GetFavoriteForums();

            return View(model);
        }

        public async Task<ActionResult> AddToFavorite(Guid forumId)
        {
            var model = _scope.Resolve<FavoriteForumModel>();
            await model.ResolveAsync(_scope);
            model.AddToFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        public async Task<ActionResult> RemoveFromFavorite(Guid forumId)
        {
            var model = _scope.Resolve<FavoriteForumModel>();
            await model.ResolveAsync(_scope);
            model.RemoveFromFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}