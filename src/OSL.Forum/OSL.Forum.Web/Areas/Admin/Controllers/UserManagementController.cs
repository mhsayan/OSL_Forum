using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.UserManagement;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class UserManagementController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UserManagementController));
        private readonly ILifetimeScope _scope;

        public UserManagementController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        // GET: Admin/UserManagement
        public ActionResult AssignRole()
        {
            var model = _scope.Resolve<AssignRoleModel>();
            model.Resolve(_scope);
            model.GetUsers();
            model.AdminRoles();
            model.SuperAdminRoles();

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AssignRole(AssignRoleModel model)
        {
            model.Resolve(_scope);
            
            

            return View();
        }
    }
}