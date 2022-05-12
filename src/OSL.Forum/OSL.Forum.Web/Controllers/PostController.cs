using log4net;
using Microsoft.AspNet.Identity;
using OSL.Forum.Common.Enums;
using OSL.Forum.Common.Utilities;
using OSL.Forum.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;
using System;
using System.Web.Mvc;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ILog _logger;
        private readonly ITopicService _topicService;
        private readonly IPostService _postService;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IProfileService _profileService;

        public PostController()
        {
            _logger = LogManager.GetLogger(typeof(PostController));
            _dateTimeUtility = new DateTimeUtility();
            _profileService = new ProfileService();
            _topicService = new TopicService();
            _postService = new PostService();
        }

        public ActionResult Edit(long postId)
        {
            var post = _postService.GetPost(postId);
            var model = new PostModel(post);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ApplicationUserId != User.Identity.GetUserId())
                return View(model);

            try
            {
                model.Time = _dateTimeUtility.Now;

                var post = model.PostBuilder();
                post.Status = Status.Pending.ToString();

                _postService.EditPost(post);
                _topicService.UpdateModificationDate(model.TopicId, model.Time);

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
            var model = new PostModel
            {
                BoTopic = _topicService.GetTopic(topicId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.BoTopic = _topicService.GetTopic(model.TopicId);

                if (model.BoTopic.ActivityStatus != ActivityStatus.Active.ToString())
                    return View(model);

                var user = _profileService.GetUser();
                model.Time = _dateTimeUtility.Now;

                var post = new BO.Post
                {
                    Name = model.Name,
                    Description = model.Description,
                    TopicId = model.TopicId,
                    CreationDate = model.Time,
                    ModificationDate = model.Time,
                    ApplicationUserId = user.Id,
                    Status = model.BoTopic.ApprovalType == ApprovalType.Auto.ToString()
                        ? Status.Approved.ToString()
                        : Status.Pending.ToString()
                };

                _postService.CreatePost(post);
                _topicService.UpdateModificationDate(model.TopicId, model.Time);

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
            _postService.DeletePost(postId);

            return RedirectToAction("Details", "Topic", new { topicId = topicId });
        }
    }
}