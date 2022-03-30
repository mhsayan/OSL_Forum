using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> AssignRole()
        {
            var model = _scope.Resolve<AssignRoleModel>();
            await model.ResolveAsync(_scope);
            model.GetUsers();
            model.AdminRoles();
            model.SuperAdminRoles();

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AssignRole(AssignRoleModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await model.ResolveAsync(_scope);
                await model.AddUserToRoleAsync();

                return Redirect(nameof(AssignRole));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Error("User role assign failed.");
                _logger.Error(ex.Message);

                return View(model);
            }
        }
    }
}