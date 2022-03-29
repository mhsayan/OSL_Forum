using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using OSL.Forum.Web.Seeds;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models
{
    public class BaseModel
    {
        public bool ShowDashboard { get; set; }
        private IProfileService _profileService;
        private ILifetimeScope _scope;

        public BaseModel()
        {

        }

        public BaseModel(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public virtual async Task Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();

            await GetUserInfo();
        }

        private async Task GetUserInfo()
        {
            var isAuthenticated = _profileService.IsAuthenticated();

            if (!isAuthenticated)
            {
                ShowDashboard = false;
                return;
            }

            var userRoles = await _profileService.UserRolesAsync();

            if (!userRoles.Contains(Roles.User.ToString()))
                ShowDashboard = true;
        }
    }
}