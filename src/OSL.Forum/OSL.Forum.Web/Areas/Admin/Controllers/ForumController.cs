using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.Forum;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ForumController : Controller
    {
        private readonly ILog _logger;

        public ForumController()
        {
            _logger = LogManager.GetLogger(typeof(ForumController));
        }

        public ActionResult Create(long categoryId)
        {
            var model = new CreateForumModel();
            model.GetCategory(categoryId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateForumModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
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

        public ActionResult Edit(long id, long categoryId)
        {
            var model = new EditForumModel();
            model.GetForum(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditForumModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
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

        public ActionResult Delete(long id, long categoryId)
        {
            var model = new EditForumModel();
            model.Delete(id);

            return RedirectToAction("Details", "Category", new { id = categoryId });
        }
    }
}