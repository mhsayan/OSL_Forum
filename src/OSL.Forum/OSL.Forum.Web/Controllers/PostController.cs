using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Models.Post;

namespace OSL.Forum.Web.Controllers
{
    public class PostController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PostController));
        private readonly ILifetimeScope _scope;

        public PostController(ILifetimeScope scope)
        {
            _scope = scope;
        }
        // GET: Post
        public ActionResult Topic(Guid id)
        {
            var model = _scope.Resolve<TopicViewModel>();
            model.Resolve(_scope);
            model.GetForum(id);
            model.GetCategory();
            model.LoadUserInfo();

            return View(model);
        }
    }
}