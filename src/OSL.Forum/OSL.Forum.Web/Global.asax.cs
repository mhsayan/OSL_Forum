using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Seeds;

namespace OSL.Forum.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityHelper.SeedIdentities(context);
        }
    }
}
