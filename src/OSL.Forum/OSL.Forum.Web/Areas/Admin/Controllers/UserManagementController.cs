using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Web.Areas.Admin.Models.UserManagement;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class UserManagementController : Controller
    {
        private readonly ILog _logger;

        public UserManagementController()
        {
            _logger = LogManager.GetLogger(typeof(UserManagementController));
        }

        public async Task<ActionResult> AssignRole()
        {
            var model = new AssignRoleModel();
            await model.Resolve();
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
                await model.Resolve();
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