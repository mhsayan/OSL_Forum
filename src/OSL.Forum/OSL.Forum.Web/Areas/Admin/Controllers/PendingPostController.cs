using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Core.Enums;
using OSL.Forum.Web.Areas.Admin.Models.PendingPost;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class PendingPostController : Controller
    {
        private readonly ILog _logger;

        public PendingPostController(ILifetimeScope scope)
        {
            _logger = LogManager.GetLogger(typeof(PendingPostController));
        }

        // GET: Admin/PendingPost
        public async Task<ActionResult> Index(int? page)
        {
            var model = new PendingPostListModel();
            await model.Resolve();
            model.GetPendingPostList(page);

            return View(model);
        }

        public async Task<ActionResult> Post(long postId)
        {
            var model = new PendingPostDetailsModel();
            await model.Resolve();
            model.GetPendingPost(postId);

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Post(string button, PendingPostDetailsModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.Resolve();

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

        public async Task<ActionResult> Accept(long postId)
        {
            var model = new PendingPostListModel();
            await model.Resolve();
            model.UpdatePostStatus(postId, Status.Approved.ToString());

            return Redirect(nameof(Index));
        }

        public async Task<ActionResult> Reject(long postId)
        {
            var model = new PendingPostListModel();
            await model.Resolve();
            model.UpdatePostStatus(postId, Status.Rejected.ToString());

            return Redirect(nameof(Index));
        }
    }
}