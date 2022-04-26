using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Seeds;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.UserManagement
{
    public class AssignRoleModel : BaseModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserRole { get; set; }
        public List<SelectListItem> ApplicationUserList { get; set; }
        public List<SelectListItem> AdminRoleList { get; set; }
        public List<SelectListItem> SuperAdminRoleList { get; set; }
        private IProfileService _profileService;

        public AssignRoleModel()
        {
        }

        public override async Task Resolve()
        {
            _profileService = ProfileService.Create();

            await base.Resolve();
        }

        public void GetUsers()
        {
            ApplicationUserList = _profileService.GetUserList();
        }

        public void SuperAdminRoles()
        {
            SuperAdminRoleList = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = Roles.Admin.ToString(),
                    Value = Roles.Admin.ToString()
                }
            };

            SuperAdminRoleList.AddRange(AdminRoleList);

        }

        public void AdminRoles()
        {
            AdminRoleList = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = Roles.Moderator.ToString(),
                    Value = Roles.Moderator.ToString()
                },
                new SelectListItem
                {
                    Text = Roles.User.ToString(),
                    Value = Roles.User.ToString()
                },
            };
        }

        public async Task AddUserToRoleAsync()
        {
            var applicationUserRole = new ApplicationUserRole()
            {
                UserId = this.UserId,
                UserRole = this.UserRole
            };

            await _profileService.AddUserToRoleAsync(applicationUserRole);
        }
    }
}