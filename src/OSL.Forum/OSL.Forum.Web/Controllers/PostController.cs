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

        public async Task<ActionResult> Edit(long postId)
        {
            var model = _scope.Resolve<EditPostModel>();
            await model.ResolveAsync(_scope);
            model.GetPost(postId);
            //model.GetTopic();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPostModel model)
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

        public async Task<ActionResult> Create(long topicId)
        {
            var model = _scope.Resolve<CreatePostModel>();
            await model.ResolveAsync(_scope);
            model.GetTopic(topicId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.ResolveAsync(_scope);
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

        public async Task<ActionResult> Delete(long postId, long topicId)
        {
            var model = _scope.Resolve<EditPostModel>();
            await model.ResolveAsync(_scope);
            model.Delete(postId);

            return RedirectToAction("Details", "Topic", new { topicId = topicId });
        }
    }
}