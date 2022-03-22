﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.Forum;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class ForumController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CategoryController));
        private readonly ILifetimeScope _scope;

        public ForumController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Admin/Forum
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(Guid categoryId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateForumModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                model.Resolve(_scope);
                model.Create();

                return Redirect(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("New Category Create failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }
    }
}