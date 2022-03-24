using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Models.Post;
using OSL.Forum.Web.Models.Topic;

namespace OSL.Forum.Web.Controllers
{
    public class PostController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PostController));
        private readonly ILifetimeScope _scope;

        public PostController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public ActionResult Edit(Guid postId, Guid topicId)
        {
            var model = _scope.Resolve<EditPostModel>();
            model.Resolve(_scope);
            model.GetPost(postId);

            return View(model);
        }

        [Authorize]
        public ActionResult Create(Guid forumId)
        {
            var model = _scope.Resolve<CreateTopicModel>();
            model.Resolve(_scope);
            model.GetForum(forumId);
            model.GetCategory();

            return View(model);
        }

        //[Authorize]
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

        public ActionResult Delete()
        {
            return null;
        }
    }
}