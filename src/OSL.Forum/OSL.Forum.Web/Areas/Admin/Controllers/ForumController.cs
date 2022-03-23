using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.Forum;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
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

                return RedirectToAction("Details", "Category", new { id = model.CategoryId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("New Forum Create failed.");
                _logger.Error(ex.Message);

                return View();
            }
        }

        public ActionResult Edit(Guid id, Guid categoryId)
        {
            return id == Guid.Empty ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditForumModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                model.Resolve(_scope);
                model.Edit();

                return RedirectToAction("Details", "Category", new { id = model.CategoryId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Forum edit failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public ActionResult Delete(Guid id, Guid categoryId)
        {
            var model = _scope.Resolve<EditForumModel>();
            model.Resolve(_scope);
            model.Delete(id);

            return RedirectToAction("Details", "Category", new { id = categoryId });
        }
    }
}