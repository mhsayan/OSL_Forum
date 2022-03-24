﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Topics(Guid id)
        {
            var model = _scope.Resolve<TopicViewModel>();
            model.Resolve(_scope);
            model.GetForum(id);
            model.GetCategory();
            model.LoadUserInfo();

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
                model.GetForum(model.ForumId);
                model.GetCategory();
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

        public ActionResult Details(Guid topicId)
        {
            var model = _scope.Resolve<TopicDetailsModel>();
            model.Resolve(_scope);
            model.GetTopic(topicId);
            model.GetForum();
            model.GetCategory();

            return View(model);
        }
    }
}