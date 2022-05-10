using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Web.Models.Home;

namespace OSL.Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _logger;

        public HomeController()
        {
            _logger = LogManager.GetLogger(typeof(HomeController));
        }


        public ActionResult Index(int? page)
        {
            var model = new IndexViewModel();
            model.GetCategories(page);
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                model.GetFavoriteForums();

            return View(model);
        }

        public ActionResult Details(int? page, long id)
        {
            var model = new DetailsModel();
            model.GetCategory(id);
            model.GetForums(id, page);

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