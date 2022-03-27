using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Seeds;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.UserManagement
{
    public class AssignRoleModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public Roles UserRole { get; set; }
        public IList<ApplicationUser> ApplicationUsers { get; set; }
        public List<SelectListItem> ApplicationUserList { get; set; }
        private ILifetimeScope _scope;
        private IProfileService _profileService;

        public AssignRoleModel()
        {
        }

        public AssignRoleModel(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
        }

        public void GetUsers()
        {
            ApplicationUserList = _profileService.GetUserList();
        }
    }
}