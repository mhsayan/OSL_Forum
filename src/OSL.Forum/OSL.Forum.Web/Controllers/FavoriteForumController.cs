using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult AddToFavorite(Guid forumId)
        {
            var model = _scope.Resolve<FavoriteForumModel>();
            model.Resolve(_scope);
            model.AddToFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        public ActionResult RemoveFromFavorite(Guid forumId)
        {
            var model = _scope.Resolve<FavoriteForumModel>();
            model.Resolve(_scope);
            model.RemoveFromFavorite(forumId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}