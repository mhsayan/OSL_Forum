using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Services
{
    public class ProfileService : IProfileService
    {
        private static readonly UserStore<ApplicationUser> UserStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
        private static readonly ApplicationUserManager UserManager = new ApplicationUserManager(UserStore);

        public string UserID()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        public ApplicationUser GetUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            return user ?? null;
        }

        public ApplicationUser GetUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var user = UserManager.FindById(userId);

            return user;
        }

        public async Task<IList<string>> UserRoles()
        {
            var userId = UserID();

            return await UserManager.GetRolesAsync(userId);
        }

        public bool Owner(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException(nameof(userId));

            var user = GetUser();

            if (user == null)
                return false;

            return userId == user.Id;
        }

        public bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}