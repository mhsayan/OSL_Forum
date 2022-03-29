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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));
        private readonly ILifetimeScope _scope;

        public HomeController(ILifetimeScope scope)
        {
            _scope = scope;
        }


        public async Task<ActionResult> Index()
        {
            var model = _scope.Resolve<IndexViewModel>();
            await model.Resolve(_scope);
            model.GetCategories();
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                model.GetFavoriteForums();

            return View(model);
        }

        public ActionResult Details(Guid id)
        {
            var model = _scope.Resolve<DetailsModel>();
            model.Resolve(_scope);
            model.GetCategory(id);

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