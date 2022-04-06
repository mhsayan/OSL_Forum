using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Models.Topic;

namespace OSL.Forum.Web.Controllers
{
    public class TopicController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TopicController));
        private readonly ILifetimeScope _scope;

        public TopicController(ILifetimeScope scope)
        {
            _scope = scope;
        }
        // GET: Post
        public async Task<ActionResult> Topics(int? page, Guid id)
        {
            var model = _scope.Resolve<TopicViewModel>();
            await model.ResolveAsync(_scope);
            model.GetForum(id);
            model.GetTopics(page, id);
            model.LoadUserInfo();

            if (model.IsAuthenticated)
                model.FavoriteForumStatus();

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Create(Guid forumId)
        {
            var model = _scope.Resolve<CreateTopicModel>();
            await model.ResolveAsync(_scope);
            model.GetForum(forumId);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTopicModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await model.ResolveAsync(_scope);
                model.GetForum(model.ForumId);
                model.Create();
                model.CreatePost();

                return RedirectToAction("Topics", "Topic", new { id = model.ForumId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("New Topic Creation failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public async Task<ActionResult> Details(Guid topicId)
        {
            var model = _scope.Resolve<TopicDetailsModel>();
            await model.ResolveAsync(_scope);
            model.GetTopic(topicId);
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                await model.GetUserRolesAsync();

            return View(model);
        }

        public async Task<ActionResult> Delete(Guid topicId, Guid forumId)
        {
            var model = _scope.Resolve<TopicViewModel>();
            await model.ResolveAsync(_scope);
            model.Delete(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}