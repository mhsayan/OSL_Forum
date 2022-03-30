using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.Category;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CategoryController));
        private readonly ILifetimeScope _scope;

        public CategoryController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Category
        [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
        public async Task<ActionResult> Index()
        {
            var model = _scope.Resolve<CategoriesModel>();
            await model.ResolveAsync(_scope);
            ViewBag.Categories = model.GetCategories();
            await model.LoadUserInfo();

            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = _scope.Resolve<CreateCategoryModel>();
            await model.ResolveAsync(_scope);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.ResolveAsync(_scope);
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

        public async Task<ActionResult> Edit(Guid id)
        {
            var model = _scope.Resolve<EditCategoryModel>();
            await model.ResolveAsync(_scope);
            model.GetCategory(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.ResolveAsync(_scope);
                model.Edit();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("Category edit failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var model = _scope.Resolve<CategoriesModel>();
            await model.ResolveAsync(_scope);
            model.Delete(id);

            return RedirectToAction(nameof(Index), "Category");
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
        public async Task<ActionResult> Details(Guid id)
        {
            var model = _scope.Resolve<CategoryDetailsModel>();
            await model.ResolveAsync(_scope);
            model.GetCategory(id);
            await model.LoadUserInfo();

            return View(model);
        }
    }
}