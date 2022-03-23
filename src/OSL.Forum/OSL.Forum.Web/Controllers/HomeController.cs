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


        public ActionResult Index()
        {
            var model = _scope.Resolve<IndexViewModel>();
            model.Resolve(_scope);
            ViewBag.Categories = model.GetCategories();

            return View(model);
        }

        public async Task<ActionResult> Details(Guid id)
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