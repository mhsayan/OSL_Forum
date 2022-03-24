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
        // GET: Post
        public ActionResult Topic(Guid id)
        {
            var model = _scope.Resolve<TopicViewModel>();
            model.Resolve(_scope);
            model.GetForum(id);
            model.GetCategory();
            model.LoadUserInfo();

            return View(model);
        }

        [Authorize]
        public ActionResult Create(Guid id)
        {
            var model = _scope.Resolve<CreateTopicModel>();
            model.Resolve(_scope);

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTopicModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                model.Resolve(_scope);
                model.Create();
                model.CreatePost();

                return RedirectToAction("Details", "Home", new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("New Topic Creation failed.");
                _logger.Error(ex.Message);

                return View();
            }
        }
    }
}