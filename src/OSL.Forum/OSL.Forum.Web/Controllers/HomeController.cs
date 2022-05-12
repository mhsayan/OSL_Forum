using log4net;
using OSL.Forum.Common.Utilities;
using OSL.Forum.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;
using System.Linq;
using System.Web.Mvc;

namespace OSL.Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _logger;
        private readonly ICategoryService _categoryService;
        private readonly IForumService _forumService;
        private readonly IProfileService _profileService;
        private readonly IFavoriteForumService _favoriteForumService;

        public HomeController()
        {
            _logger = LogManager.GetLogger(typeof(HomeController));
            _categoryService = new CategoryService();
            _favoriteForumService = new FavoriteForumService();
            _profileService = new ProfileService();
            _forumService = new ForumService();
        }


        public ActionResult Index(int? page)
        {
            var totalItem = _categoryService.GetCategoryCount();

            var model = new HomeModel
            {
                Pager = new Pager(totalItem, page)
            };

            model.Categories = _categoryService.GetCategories(model.Pager.CurrentPage, model.Pager.PageSize);

            model.IsAuthenticated = _profileService.IsAuthenticated();

            if (!model.IsAuthenticated) 
                return View(model);

            var user = _profileService.GetUser();
            model.FavoriteForums = _favoriteForumService.GetUserFavoriteForums(user.Id).Take(4).ToList();

            return View(model);
        }

        public ActionResult Details(int? page, long id)
        {
            var totalItem = _forumService.GetForumCount(id);
            var model = new HomeModel
            {
                Category = _categoryService.GetCategory(id),
                Pager = new Pager(totalItem, page)
            };
            model.Forums = _forumService.GetForums(model.Pager.CurrentPage, model.Pager.PageSize, id);

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}