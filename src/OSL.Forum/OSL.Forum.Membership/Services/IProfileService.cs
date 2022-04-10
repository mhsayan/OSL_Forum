using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Membership.Entities;
using OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.Membership.Services
{
    public interface IProfileService
    {
        string UserID();
        ApplicationUser GetUser();
        ApplicationUser GetUser(string userId);
        Task<IList<string>> UserRolesAsync();
        bool Owner(string userId);
        bool IsAuthenticated();
        Task EditProfileAsync(ApplicationUser applicationUser);
        List<SelectListItem> GetUserList();
        Task AddUserToRoleAsync(ApplicationUserRole applicationUserRole);
    }
}
