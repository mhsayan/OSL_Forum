using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Areas.Admin.Models;
using OSL.Forum.Web.Areas.Admin.Models.PendingPost;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class PendingPostController : Controller
    {
        private readonly ILog _logger;
        private readonly IPostService _postService;
        private readonly IProfileService _profileService;
        private readonly ITopicService _topicService;

        public PendingPostController()
        {
            _logger = LogManager.GetLogger(typeof(PendingPostController));
            _postService = new PostService();
            _profileService = new ProfileService();
            _topicService = new TopicService();
        }

        // GET: Admin/PendingPost
        public ActionResult Index(int? page)
        {
            var model = new PendingPostModel();

            var totalPendingPost = _postService.GetPendingPostCount();

            model.Pager = new Pager(totalPendingPost, page);
            model.Posts = _postService.PendingPosts(model.Pager.CurrentPage, model.Pager.PageSize);

            foreach (var post in model.Posts)
            {
                post.OwnerName = _profileService.GetUser(post.ApplicationUserId).Name;
            }

            return View(model);
        }

        public ActionResult Post(long postId)
        {
            var model = new PendingPostModel
            {
                Post = _postService.GetPost(postId)
            };

            model.Post.OwnerName = _profileService.GetUser(model.Post.ApplicationUserId).Name;
            model.Id = model.Post.Id;
            model.Name = model.Post.Name;
            model.Description = model.Post.Description;
            model.TopicId = model.Post.TopicId;

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Post(string button, PendingPostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                switch (button)
                {
                    case "Accept":
                        _postService.UpdatePostStatus(model.Id, Status.Approved.ToString());
                        break;
                    case "Reject":
                        _postService.UpdatePostStatus(model.Id, Status.Rejected.ToString());
                        break;
                }
                
                if (model.ApprovalStatus)
                    _topicService.UpdateTopicApprovalType(model.TopicId);

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
            _postService.UpdatePostStatus(postId, Status.Approved.ToString());

            return Redirect(nameof(Index));
        }

        public ActionResult Reject(long postId)
        {
            _postService.UpdatePostStatus(postId, Status.Rejected.ToString());

            return Redirect(nameof(Index));
        }
    }
}