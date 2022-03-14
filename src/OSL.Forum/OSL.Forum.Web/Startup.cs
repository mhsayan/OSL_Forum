using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OSL.Forum.Web.Startup))]
namespace OSL.Forum.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
