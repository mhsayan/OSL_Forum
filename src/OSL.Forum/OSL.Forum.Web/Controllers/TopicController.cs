using log4net;
using OSL.Forum.Common.Enums;
using OSL.Forum.Common.Utilities;
using OSL.Forum.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly ILog _logger;
        private readonly IForumService _forumService;
        private readonly ITopicService _topicService;
        private readonly IFavoriteForumService _favoriteForumService;
        private readonly IProfileService _profileService;
        private readonly IPostService _postService;
        private readonly IDateTimeUtility _dateTimeUtility;

        public TopicController()
        {
            _logger = LogManager.GetLogger(typeof(HomeController));
            _forumService = new ForumService();
            _favoriteForumService = new FavoriteForumService();
            _profileService = new ProfileService();
            _topicService = new TopicService();
            _postService = new PostService();
            _dateTimeUtility = new DateTimeUtility();
        }
        // GET: Post
        public async Task<ActionResult> Topics(int? page, long id)
        {
            var model = new TopicModel
            {
                Forum = _forumService.GetForum(id)
            };

            var totalItem = _topicService.GetTopicCount(id);
            model.Pager = new Pager(totalItem, page);
            model.Topics = _topicService.GetTopics(model.Pager.CurrentPage, model.Pager.PageSize, id);
            model.IsAuthenticated = _profileService.IsAuthenticated();

            var topicList = new List<Topic>();

            foreach (var topic in model.Topics)
            {
                topic.Owner = _profileService.Owner(topic.ApplicationUserId);
                topicList.Add(topic);
            }

            model.Topics = topicList;

            if (!model.IsAuthenticated) 
                return View(model);

            var user = _profileService.GetUser();
            var favoriteForum = _favoriteForumService.GetFavoriteForum(model.Forum.Id, user.Id);
            model.IsFavorite = favoriteForum != null;
            model.UserRoles = await _profileService.UserRolesAsync();

            return View(model);
        }

        [Authorize]
        public ActionResult Create(long forumId)
        {
            var model = new CreateTopicModel
            {
                Forum = _forumService.GetForum(forumId)
            };

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
                model.Forum = _forumService.GetForum(model.ForumId);

                model.ApplicationUser = _profileService.GetUser();
                model.Time = _dateTimeUtility.Now;
                var topic = model.TopicBuilder();
                _topicService.CreateTopic(topic);

                var topicId = _topicService.GetTopic(model.Name).Id;
                var post = model.PostBuilder(topicId);
                _postService.CreatePost(post);

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
            var model = new TopicModel();
            model.Topic = _topicService.GetTopic(topicId);
            var postList = new List<Post>();

            foreach (var topicPost in model.Topic.Posts)
            {
                if (topicPost.Status != Status.Approved.ToString()) continue;
                topicPost.Owner = _profileService.Owner(topicPost.ApplicationUserId);
                topicPost.OwnerName = _profileService.GetUser(topicPost.ApplicationUserId).Name;

                postList.Add(model.PostBuilder(topicPost));
            }

            model.Topic.Posts = postList;
            model.IsAuthenticated = _profileService.IsAuthenticated();

            if (model.IsAuthenticated)
                model.UserRoles = await _profileService.UserRolesAsync();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public ActionResult Delete(long topicId, long forumId)
        {
            _topicService.DeleteTopic(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public ActionResult Close(long topicId, long forumId)
        {
            _topicService.CloseTopic(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator, User")]
        public ActionResult Open(long topicId, long forumId)
        {
            _topicService.OpenTopic(topicId);

            return RedirectToAction("Topics", "Topic", new { id = forumId });
        }
    }
}