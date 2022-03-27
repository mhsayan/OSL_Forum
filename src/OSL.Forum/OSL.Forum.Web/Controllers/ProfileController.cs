using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using Microsoft.AspNet.Identity;
using OSL.Forum.Web.Models.Post;
using OSL.Forum.Web.Models.Profile;

namespace OSL.Forum.Web.Controllers
{
    public class ProfileController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProfileController));
        private readonly ILifetimeScope _scope;

        public ProfileController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Profile
        public ActionResult MyProfile()
        {
            var model = _scope.Resolve<ProfileDetailsModel>();
            model.Resolve(_scope);
            model.GetUserInfo();
            model.GetMyPosts();

            return View(model);
        }

        public ActionResult EditPost(Guid postId)
        {
            var model = _scope.Resolve<EditPostModel>();
            model.Resolve(_scope);
            model.GetPost(postId);
            model.GetTopic();
            model.GetForum();
            model.GetCategory();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(EditPostModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (model.ApplicationUserId != User.Identity.GetUserId())
                return View();

            try
            {
                model.Resolve(_scope);
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

        public ActionResult DeletePost(Guid postId, Guid topicId)
        {
            var model = _scope.Resolve<EditPostModel>();
            model.Resolve(_scope);
            model.Delete(postId);

            return Redirect(nameof(MyProfile));
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}