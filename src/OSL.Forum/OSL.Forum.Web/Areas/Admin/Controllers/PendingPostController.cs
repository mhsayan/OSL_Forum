using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.PendingPost;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class PendingPostController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PendingPostController));
        private readonly ILifetimeScope _scope;

        public PendingPostController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Admin/PendingPost
        public ActionResult Index()
        {
            var model = _scope.Resolve<PendingPostListModel>();
            model.Resolve(_scope);
            model.GetPendingPostList();

            return View(model);
        }

        public ActionResult Accept(Guid postId)
        {
            var model = _scope.Resolve<PendingPostListModel>();
            model.Resolve(_scope);
            model.AcceptPost(postId);

            return Redirect(nameof(Index));
        }

        public ActionResult Reject(Guid postId)
        {
            var model = _scope.Resolve<PendingPostListModel>();
            model.Resolve(_scope);
            model.RejectPost(postId);

            return Redirect(nameof(Index));
        }
    }
}