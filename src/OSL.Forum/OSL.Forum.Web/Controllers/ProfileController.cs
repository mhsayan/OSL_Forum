using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using Microsoft.AspNet.Identity;
using OSL.Forum.Web.Models.Post;
using OSL.Forum.Web.Models.Profile;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILog _logger;

        public ProfileController()
        {
            _logger = LogManager.GetLogger(typeof(ProfileController));
        }

        // GET: Profile
        public async Task<ActionResult> MyProfile(int? page)
        {
            var model = new ProfileDetailsModel();
            await model.Resolve();
            model.GetUserInfo();
            model.GetMyPosts(page);

            return View(model);
        }

        public async Task<ActionResult> EditPost(long postId)
        {
            var model = new EditPostModel();
            await model.Resolve();
            model.GetPost(postId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(EditPostModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (model.ApplicationUserId != User.Identity.GetUserId())
                return View();

            try
            {
                await model.Resolve();
                model.EditPost();
                model.UpdateTopicModificationDate();

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

        public async Task<ActionResult> DeletePost(long postId, long topicId)
        {
            var model = new EditPostModel();
            await model.Resolve();
            model.Delete(postId);

            return Redirect(nameof(MyProfile));
        }

        public async Task<ActionResult> Edit()
        {
            var model = new EditProfileModel();
            await model.Resolve();
            model.LoadUserInfo();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditProfileModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await model.Resolve();
                await model.EditProfileAsync();

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