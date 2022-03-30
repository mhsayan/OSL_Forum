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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProfileController));
        private readonly ILifetimeScope _scope;

        public ProfileController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Profile
        public async Task<ActionResult> MyProfile()
        {
            var model = _scope.Resolve<ProfileDetailsModel>();
            await model.ResolveAsync(_scope);
            model.GetUserInfo();
            model.GetMyPosts();

            return View(model);
        }

        public async Task<ActionResult> EditPost(Guid postId)
        {
            var model = _scope.Resolve<EditPostModel>();
            await model.ResolveAsync(_scope);
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
                await model.ResolveAsync(_scope);
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

        public async Task<ActionResult> DeletePost(Guid postId, Guid topicId)
        {
            var model = _scope.Resolve<EditPostModel>();
            await model.ResolveAsync(_scope);
            model.Delete(postId);

            return Redirect(nameof(MyProfile));
        }

        public async Task<ActionResult> Edit()
        {
            var model = _scope.Resolve<EditProfileModel>();
            await model.ResolveAsync(_scope);
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
                await model.ResolveAsync(_scope);
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