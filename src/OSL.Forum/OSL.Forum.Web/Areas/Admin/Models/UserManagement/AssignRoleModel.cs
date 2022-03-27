using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.UserManagement
{
    public class AssignRoleModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
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
    }
}