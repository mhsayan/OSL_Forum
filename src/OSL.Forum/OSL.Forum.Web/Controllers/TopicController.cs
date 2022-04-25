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
        private readonly ILog _logger;

        public TopicController()
        {
            _logger = LogManager.GetLogger(typeof(HomeController));
        }
        // GET: Post
        public async Task<ActionResult> Topics(int? page, long id)
        {
            var model = new TopicViewModel();
            await model.Resolve();
            model.GetForum(id);
            model.GetTopics(page, id);
            model.LoadUserInfo();

            if (model.IsAuthenticated)
            {
                model.FavoriteForumStatus();
                await model.GetUserRolesAsync();
            }



            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Create(long forumId)
        {
            var model = new CreateTopicModel();
            await model.Resolve();
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
                await model.Resolve();
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

        public async Task<ActionResult> Details(long topicId)
        {
            var model = new TopicDetailsModel();
            await model.Resolve();
            model.GetTopic(topicId);
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                await model.GetUserRolesAsync();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public async Task<ActionResult> Delete(long topicId, long forumId)
        {
            var model = new TopicViewModel();
            await model.Resolve();
            model.Delete(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public async Task<ActionResult> Close(long topicId, long forumId)
        {
            var model = new TopicViewModel();
            await model.Resolve();
            model.Close(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public async Task<ActionResult> Open(long topicId, long forumId)
        {
            var model = new TopicViewModel();
            await model.Resolve();
            model.Open(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}