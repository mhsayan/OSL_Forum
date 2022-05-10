using System.Threading.Tasks;
using OSL.Forum.Web.Seeds;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models
{
    public class BaseModel
    {
        public bool ShowDashboard { get; set; }
        private IProfileService _profileService;

        public BaseModel()
        {
        }

        public virtual async Task Resolve()
        {
            _profileService = new ProfileService();
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