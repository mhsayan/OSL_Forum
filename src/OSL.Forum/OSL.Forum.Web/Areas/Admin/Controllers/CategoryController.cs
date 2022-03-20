using System;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Models.Category;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CategoryController));
        private readonly ILifetimeScope _scope;

        public CategoryController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryModel model)
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

                return View(model);
            }
        }
    }
}