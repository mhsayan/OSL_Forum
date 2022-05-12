using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Entities.BusinessObjects;
using OSL.Forum.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OSL.Forum.Web.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationUserManager _userManager;

        public ProfileService()
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            _userManager = new ApplicationUserManager(userStore);
        }
        
        public string UserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        public ApplicationUser GetUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = _userManager.FindById(userId);

            return user ?? null;
        }

        public ApplicationUser GetUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var user = _userManager.FindById(userId);

            return user;
        }

        public async Task<IList<string>> UserRolesAsync()
        {
            var userId = UserId();

            return await _userManager.GetRolesAsync(userId);
        }

        public async Task<IList<string>> UserRolesAsync(string userId)
        {
            return await _userManager.GetRolesAsync(userId);
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

        public async Task EditProfileAsync(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                throw new ArgumentNullException(nameof(applicationUser));

            var user = _userManager.FindById(applicationUser.Id);

            if (user.Name == applicationUser.Name)
                throw new InvalidOperationException("The name is the same as your previous name.");

            user.Name = applicationUser.Name;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new InvalidOperationException("User profile update failed.");
        }

        public async Task AddUserToRoleAsync(ApplicationUserRole applicationUserRole)
        {
            if (applicationUserRole == null)
                throw new ArgumentNullException(nameof(applicationUserRole));

            await RemoveUserFromRolesAsync(applicationUserRole.UserId);

            var result = await _userManager.AddToRoleAsync(applicationUserRole.UserId, applicationUserRole.UserRole);

            if (!result.Succeeded)
                throw new InvalidOperationException("Role assign failed.");
        }

        public List<SelectListItem> GetUserList()
        {
            var user = GetUser();
            var superAdmin = ConfigurationManager.AppSettings["SuperAdminEmail"].ToString();

            var users = _userManager.Users.ToList()
                .Where(u => u.Email != user.Email && u.Email != superAdmin)
                .Select(u => new SelectListItem
                {
                    Text = u.Email,
                    Value = u.Id.ToString()
                }).ToList();

            return users;
        }

        public async Task RemoveUserFromRolesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var userRoles = await UserRolesAsync(userId);

            foreach (var userRole in userRoles)
            {
                var result = await _userManager.RemoveFromRolesAsync(userId, userRole);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Role remove failed.");
            }
        }
    }
}