using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Core.Enums;
using OSL.Forum.Web.Areas.Admin.Models.PendingPost;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class PendingPostController : Controller
    {
        private readonly ILog _logger;

        public PendingPostController()
        {
            _logger = LogManager.GetLogger(typeof(PendingPostController));
        }

        // GET: Admin/PendingPost
        public ActionResult Index(int? page)
        {
            var model = new PendingPostListModel();
            model.GetPendingPostList(page);

            return View(model);
        }

        public ActionResult Post(long postId)
        {
            var model = new PendingPostDetailsModel();
            model.GetPendingPost(postId);

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Post(string button, PendingPostDetailsModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (button == "Accept")
                    model.UpdatePostStatus(Status.Approved.ToString());

                if (button == "Reject")
                    model.UpdatePostStatus(Status.Rejected.ToString());

                model.UpdateTopicApprovalType();

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Post Status update failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public ActionResult Accept(long postId)
        {
            var model = new PendingPostListModel();
            model.UpdatePostStatus(postId, Status.Approved.ToString());

            return Redirect(nameof(Index));
        }

        public ActionResult Reject(long postId)
        {
            var model = new PendingPostListModel();
            model.UpdatePostStatus(postId, Status.Rejected.ToString());

            return Redirect(nameof(Index));
        }
    }
}