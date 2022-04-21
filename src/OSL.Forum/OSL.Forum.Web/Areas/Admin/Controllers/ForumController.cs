using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ForumController));
        private readonly ILifetimeScope _scope;

        public ForumController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task<ActionResult> Create(long categoryId)
        {
            var model = _scope.Resolve<CreateForumModel>();
            await model.ResolveAsync(_scope);
            model.GetCategory(categoryId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateForumModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.ResolveAsync(_scope);
                model.Create();

                return RedirectToAction("Details", "Category", new { id = model.CategoryId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("New Forum Create failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public async Task<ActionResult> Edit(long id, long categoryId)
        {
            var model = _scope.Resolve<EditForumModel>();
            await model.ResolveAsync(_scope);
            model.GetForum(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditForumModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.ResolveAsync(_scope);
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

        public async Task<ActionResult> Delete(long id, long categoryId)
        {
            var model = _scope.Resolve<EditForumModel>();
            await model.ResolveAsync(_scope);
            model.Delete(id);

            return RedirectToAction("Details", "Category", new { id = categoryId });
        }
    }
}