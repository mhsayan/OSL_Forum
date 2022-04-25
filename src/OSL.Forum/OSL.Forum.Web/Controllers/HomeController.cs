using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
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


        public async Task<ActionResult> Index(int? page)
        {
            var model = new IndexViewModel();
            await model.Resolve();
            model.GetCategories(page);
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                model.GetFavoriteForums();

            return View(model);
        }

        public async Task<ActionResult> Details(int? page, long id)
        {
            var model = new DetailsModel();
            await model.Resolve();
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