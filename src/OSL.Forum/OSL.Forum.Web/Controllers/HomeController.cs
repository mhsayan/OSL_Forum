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


        public async Task<ActionResult> Index(int? page)
        {
            var model = _scope.Resolve<IndexViewModel>();
            await model.ResolveAsync(_scope);
            model.GetCategories(page);
            model.UserAuthenticatedStatus();

            if (model.IsAuthenticated)
                model.GetFavoriteForums();

            return View(model);
        }

        public async Task<ActionResult> Details(int? page, long id)
        {
            var model = _scope.Resolve<DetailsModel>();
            await model.ResolveAsync(_scope);
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