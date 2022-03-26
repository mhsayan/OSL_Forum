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
            model.GetTopic();
            model.GetForum();
            model.GetCategory();

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

        public ActionResult Create(Guid forumId)
        {
            var model = _scope.Resolve<CreateTopicModel>();
            model.Resolve(_scope);
            model.GetForum(forumId);
            model.GetCategory();

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CreateTopicModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    try
        //    {
        //        model.Resolve(_scope);
        //        model.GetForum(model.ForumId);
        //        model.GetCategory();
        //        model.Create();
        //        model.CreatePost();

        //        return RedirectToAction("Topic", "Post", new { id = model.ForumId });
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        _logger.Error("New Topic Creation failed.");
        //        _logger.Error(ex.Message);

        //        return View(model);
        //    }
        //}

        public ActionResult Delete(Guid postId, Guid topicId)
        {
            var model = _scope.Resolve<EditPostModel>();
            model.Resolve(_scope);
            model.Delete(postId);

            return RedirectToAction("Details", "Topic", new { topicId = topicId });
        }
    }
}