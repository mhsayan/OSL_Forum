using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Areas.Admin.Models;
using OSL.Forum.Web.Seeds;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class UserManagementController : Controller
    {
        private readonly ILog _logger;
        private readonly IProfileService _profileService;

        public UserManagementController()
        {
            _logger = LogManager.GetLogger(typeof(UserManagementController));
            _profileService = new ProfileService();
        }

        public ActionResult AssignRole()
        {
            var model = new AssignRoleModel
            {
                ApplicationUserList = _profileService.GetUserList()
            };
            
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
                var applicationUserRole = new ApplicationUserRole()
                {
                    UserId = model.UserId,
                    UserRole = model.UserRole
                };

                await _profileService.AddUserToRoleAsync(applicationUserRole);

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