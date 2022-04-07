using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Membership.Services;
using OSL.Forum.Membership.Utilities;
using OSL.Forum.Web.Models;

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
        private ILifetimeScope _scope;
        private IProfileService _profileService;
        private IMapper _mapper;

        public AssignRoleModel()
        {
        }

        public AssignRoleModel(IProfileService profileService, IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();

            await base.ResolveAsync(_scope);
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
            var applicationUserRole = _mapper.Map<ApplicationUserRole>(this);

            await _profileService.AddUserToRoleAsync(applicationUserRole);
        }
    }
}