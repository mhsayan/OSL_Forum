using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
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
        public ActionResult Create(long forumId)
        {
            var model = new CreateTopicModel();
            model.GetForum(forumId);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTopicModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.GetForum(model.ForumId);
                model.Create();
                model.CreatePost(model.Name);

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
            model.GetTopic(topicId);
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                await model.GetUserRolesAsync();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public ActionResult Delete(long topicId, long forumId)
        {
            var model = new TopicViewModel();
            model.Delete(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public ActionResult Close(long topicId, long forumId)
        {
            var model = new TopicViewModel();
            model.Close(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public ActionResult Open(long topicId, long forumId)
        {
            var model = new TopicViewModel();
            model.Open(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}