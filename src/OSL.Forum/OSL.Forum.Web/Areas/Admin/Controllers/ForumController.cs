using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Areas.Admin.Models;
using OSL.Forum.Web.Areas.Admin.Models.Forum;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ForumController : Controller
    {
        private readonly ILog _logger;
        private readonly IForumService _forumService;
        private readonly ICategoryService _categoryService;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IProfileService _profileService;

        public ForumController()
        {
            _logger = LogManager.GetLogger(typeof(ForumController));
            _categoryService = new CategoryService();
            _forumService = new ForumService();
            _dateTimeUtility = new DateTimeUtility();
            _profileService = new ProfileService();
        }

        public ActionResult Create(long categoryId)
        {
            if (categoryId == 0)
                throw new ArgumentException("Category Id is required");

            var model = new ForumModel
            {
                BoCategory = _categoryService.GetCategory(categoryId)
            };

            if (model.BoCategory == null)
                throw new InvalidOperationException("Forum not found");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ForumModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _profileService.GetUser();

                if (user == null)
                    throw new InvalidOperationException(nameof(user));

                var time = _dateTimeUtility.Now;
                var forum = new BO.Forum()
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    ApplicationUserId = user.Id,
                    CreationDate = time,
                    ModificationDate = time
                };

                _forumService.CreateForum(forum);
                _categoryService.UpdateModificationDate(time, forum.CategoryId);

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
            if (id == 0)
                throw new ArgumentException(nameof(id));

            var model = new ForumModel
            {
                BoForum = _forumService.GetForum(id)
            };

            if (model.BoForum == null)
                throw new InvalidOperationException("Forum not found");

            model.Id = model.BoForum.Id;
            model.Name = model.BoForum.Name;
            model.CategoryId = model.BoForum.CategoryId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ForumModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _profileService.GetUser();
                var modificationDate = _dateTimeUtility.Now;

                var forum = new BO.Forum
                {
                    Id = model.Id,
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    ModificationDate = modificationDate,
                    ApplicationUserId = user.Id
                };

                _forumService.EditForum(forum);
                _categoryService.UpdateModificationDate(modificationDate, forum.CategoryId);

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
            if (id == 0)
                throw new ArgumentException("Forum Id is required");

            _forumService.DeleteForum(id);

            return RedirectToAction("Details", "Category", new { id = categoryId });
        }
    }
}