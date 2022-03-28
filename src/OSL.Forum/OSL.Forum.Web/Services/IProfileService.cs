using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Services
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
