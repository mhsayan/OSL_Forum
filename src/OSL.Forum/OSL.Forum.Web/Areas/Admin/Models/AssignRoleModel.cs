using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OSL.Forum.Web.Seeds;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models
{
    public class AssignRoleModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserRole { get; set; }
        public List<SelectListItem> ApplicationUserList { get; set; }
        public List<SelectListItem> AdminRoleList { get; set; }
        public List<SelectListItem> SuperAdminRoleList { get; set; }

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
    }
}