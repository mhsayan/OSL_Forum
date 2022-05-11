using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Areas.Admin.Models;
using OSL.Forum.Web.Areas.Admin.Models.Category;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILog _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProfileService _profileService;
        private readonly IForumService _forumService;
        private readonly IDateTimeUtility _dateTimeUtility;

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
            var model = new CategoryModel();

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var time = _dateTimeUtility.Now;
                var category = new BO.Category()
                {
                    Name = model.Name,
                    CreationDate = time,
                    ModificationDate = time
                };

                _categoryService.CreateCategory(category);

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
            if (id == 0)
                throw new ArgumentException("Category Id is required");

            var model = new CategoryModel();
            var category = _categoryService.GetCategory(id);

            if (category == null)
                throw new InvalidOperationException("Category not found");

            model.Id = category.Id;
            model.Name = category.Name;

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var category = model.CategoryBuilder();

                _categoryService.EditCategory(category);

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
            var model = new CategoryModel
            {
                Category = _categoryService.GetCategory(id)
            };

            var totalItem = _forumService.GetForumCount(id);

            model.Pager = new Pager(totalItem, page);
            model.Forums = _forumService.GetForums(model.Pager.CurrentPage, model.Pager.PageSize, id);
            model.Roles = await _profileService.UserRolesAsync();

            return View(model);
        }
    }
}