using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.Category;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILog _logger;

        public CategoryController()
        {
            _logger = LogManager.GetLogger(typeof(CategoryController));
        }

        // GET: Category
        [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
        public async Task<ActionResult> Index(int? page)
        {
            var model = new CategoriesModel();
            await model.Resolve();
            model.GetCategories(page);
            await model.LoadUserInfo();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Create()
        {
            var model = new CreateCategoryModel();
            await model.Resolve();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.Resolve();
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

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Edit(long id)
        {
            var model = new EditCategoryModel();
            await model.Resolve();
            model.GetCategory(id);

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.Resolve();
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

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Delete(long id)
        {
            var model = new CategoriesModel();
            await model.Resolve();
            model.Delete(id);

            return RedirectToAction(nameof(Index), "Category");
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
        public async Task<ActionResult> Details(int? page, long id)
        {
            var model = new CategoryDetailsModel();
            await model.Resolve();
            model.GetCategory(id);
            model.GetForums(id, page);
            await model.LoadUserInfo();

            return View(model);
        }
    }
}