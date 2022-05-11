using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using Microsoft.AspNet.Identity;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Models.Post;
using OSL.Forum.Web.Models.Profile;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILog _logger;
        private readonly IProfileService _profileService;
        private readonly IPostService _postService;
        private readonly ITopicService _topicService;
        private readonly IDateTimeUtility _dateTimeUtility;

        public ProfileController()
        {
            _logger = LogManager.GetLogger(typeof(ProfileController));
            _profileService = new ProfileService();
            _postService = new PostService();
            _topicService = new TopicService();
            _dateTimeUtility = new DateTimeUtility();
        }

        // GET: Profile
        public ActionResult MyProfile(int? page)
        {
            var model = new ProfileModel
            {
                ApplicationUser = _profileService.GetUser()
            };

            var userTotalPost = _postService.UserPostCount(model.ApplicationUser.Id);

            model.Pager = new Pager(userTotalPost, page);
            model.Posts = _postService.GetMyPosts(model.Pager.CurrentPage, model.Pager.PageSize, model.ApplicationUser.Id);

            return View(model);
        }

        public ActionResult EditPost(long postId)
        {
            var post = _postService.GetPost(postId);
            var model = new PostModel(post);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(PostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ApplicationUserId != User.Identity.GetUserId())
                return View(model);

            try
            {
                model.Time = _dateTimeUtility.Now;

                var post = model.PostBuilder();
                post.Status = Status.Pending.ToString();

                _postService.EditPost(post);
                _topicService.UpdateModificationDate(model.TopicId, model.Time);

                return Redirect(nameof(MyProfile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Post Edit failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public ActionResult DeletePost(long postId, long topicId)
        {
            _postService.DeletePost(postId);

            return Redirect(nameof(MyProfile));
        }

        public ActionResult Edit()
        {
            var user = _profileService.GetUser();
            var model = new ProfileModel(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProfileModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                var user = model.UserBuilder();
                await _profileService.EditProfileAsync(user);

                return Redirect(nameof(MyProfile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("User profile edit failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }
    }
}