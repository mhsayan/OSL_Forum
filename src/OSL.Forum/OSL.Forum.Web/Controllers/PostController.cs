using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using Microsoft.AspNet.Identity;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Models.Post;
using OSL.Forum.Web.Models.Topic;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PostController));
        private readonly ILifetimeScope _scope;

        public PostController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public ActionResult Edit(Guid postId)
        {
            var model = _scope.Resolve<EditPostModel>();
            model.Resolve(_scope);
            model.GetPost(postId);
            //model.GetTopic();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPostModel model)
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

                return RedirectToAction("Details", "Topic", new { topicId = model.TopicId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Post Edit failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public ActionResult Create(Guid topicId)
        {
            var model = _scope.Resolve<CreatePostModel>();
            model.Resolve(_scope);
            model.GetTopic(topicId);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                model.Resolve(_scope);
                model.GetTopic(model.TopicId);
                model.CreatePost();
                model.UpdateTopicModificationDate();

                return RedirectToAction("Details", "Topic", new { topicId = model.TopicId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Post Reply failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public ActionResult Delete(Guid postId, Guid topicId)
        {
            var model = _scope.Resolve<EditPostModel>();
            model.Resolve(_scope);
            model.Delete(postId);

            return RedirectToAction("Details", "Topic", new { topicId = topicId });
        }
    }
}