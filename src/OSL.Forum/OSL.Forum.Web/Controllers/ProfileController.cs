using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Models.Profile;

namespace OSL.Forum.Web.Controllers
{
    public class ProfileController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProfileController));
        private readonly ILifetimeScope _scope;

        public ProfileController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Profile
        public ActionResult MyProfile()
        {
            var model = _scope.Resolve<ProfileDetailsModel>();
            model.Resolve(_scope);
            model.GetUserInfo();

            return View(model);
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}