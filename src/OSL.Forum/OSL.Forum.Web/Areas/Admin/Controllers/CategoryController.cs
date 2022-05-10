using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Areas.Admin.Models;
using OSL.Forum.Web.Areas.Admin.Models.Category;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILog _logger;
        private ICategoryService _categoryService;
        private IProfileService _profileService;
        private IForumService _forumService;
        private IDateTimeUtility _dateTimeUtility;

        public CategoryController()
        {
            _logger = LogManager.GetLogger(typeof(CategoryController));
            _categoryService = new CategoryService();
            _forumService = new ForumService();
            _profileService = new ProfileService();
            _dateTimeUtility = new DateTimeUtility();
        }

        // GET: Category
        [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
        public async Task<ActionResult> Index(int? page)
        {
            var totalItem = _categoryService.GetCategoryCount();

            var model = new CategoryModel
            {
                Roles = await _profileService.UserRolesAsync(),
                Pager = new Pager(totalItem, page),
            };

            model.Categories = _categoryService.GetCategories(model.Pager.CurrentPage, model.Pager.PageSize);
            
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            var model = new CreateCategoryModel();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
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
        public ActionResult Edit(long id)
        {
            var model = new EditCategoryModel();
            model.GetCategory(id);

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
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
        public ActionResult Delete(long id)
        {
            _categoryService.DeleteCategory(id);

            return RedirectToAction(nameof(Index), "Category");
        }

        [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
        public async Task<ActionResult> Details(int? page, long id)
        {
            var model = new CategoryDetailsModel();
            model.GetCategory(id);
            model.GetForums(id, page);
            await model.LoadUserInfo();

            return View(model);
        }
    }
}