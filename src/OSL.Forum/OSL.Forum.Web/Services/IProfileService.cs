using OSL.Forum.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Services
{
    public interface IProfileService
    {
        string UserId();
        ApplicationUser GetUser();
        ApplicationUser GetUser(string userId);
        Task<IList<string>> UserRolesAsync(string userId);
        Task<IList<string>> UserRolesAsync();
        bool Owner(string userId);
        bool IsAuthenticated();
        Task EditProfileAsync(ApplicationUser applicationUser);
        List<SelectListItem> GetUserList();
        Task AddUserToRoleAsync(ApplicationUserRole applicationUserRole);
    }
}
