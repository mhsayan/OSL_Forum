using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using Microsoft.AspNet.Identity;
using OSL.Forum.Web.Models.Post;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ILog _logger;

        public PostController()
        {
            _logger = LogManager.GetLogger(typeof(PostController));
        }

        public ActionResult Edit(long postId)
        {
            var model = new EditPostModel();
            model.GetPost(postId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ApplicationUserId != User.Identity.GetUserId())
                return View(model);

            try
            {
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

        public ActionResult Create(long topicId)
        {
            var model = new CreatePostModel();
            model.GetTopic(topicId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
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

        public ActionResult Delete(long postId, long topicId)
        {
            var model = new EditPostModel();
            model.Delete(postId);

            return RedirectToAction("Details", "Topic", new { topicId = topicId });
        }
    }
}