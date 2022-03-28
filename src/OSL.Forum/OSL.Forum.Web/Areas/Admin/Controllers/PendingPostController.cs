using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Core.Enums;
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

        public ActionResult Post(Guid postId)
        {
            var model = _scope.Resolve<PendingPostDetailsModel>();
            model.Resolve(_scope);
            model.GetPendingPost(postId);

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Post(string button, PendingPostDetailsModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                model.Resolve(_scope);

                if (button == "Accept")
                    model.UpdatePostStatus(Status.Approved.ToString());

                if (button == "Reject")
                    model.UpdatePostStatus(Status.Rejected.ToString());

                model.UpdateTopicApprovalType();

                return Redirect(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Post Status update failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public ActionResult Accept(Guid postId)
        {
            var model = _scope.Resolve<PendingPostListModel>();
            model.Resolve(_scope);
            model.UpdatePostStatus(postId, Status.Approved.ToString());

            return Redirect(nameof(Index));
        }

        public ActionResult Reject(Guid postId)
        {
            var model = _scope.Resolve<PendingPostListModel>();
            model.Resolve(_scope);
            model.UpdatePostStatus(postId, Status.Rejected.ToString());

            return Redirect(nameof(Index));
        }
    }
}