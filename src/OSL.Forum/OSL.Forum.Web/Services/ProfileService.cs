using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Services
{
    public class ProfileService : IProfileService
    {
        private IMapper _mapper;
        private static readonly UserStore<ApplicationUser> UserStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
        private static readonly ApplicationUserManager UserManager = new ApplicationUserManager(UserStore);

        public ProfileService(IMapper mapper)
        {
            _mapper = mapper;
        }

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

        public async Task<IList<string>> UserRolesAsync()
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

        public async Task EditProfileAsync(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                throw new ArgumentNullException(nameof(applicationUser));

            var user = UserManager.FindById(applicationUser.Id);

            if (user.Name == applicationUser.Name)
                throw new InvalidOperationException("The name is the same as your previous name.");

            user.Name = applicationUser.Name;

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new InvalidOperationException("User profile update failed.");
        }

        public async Task AddUserToRole(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var result = await UserManager.AddToRoleAsync(userId, role);

            if (!result.Succeeded)
                throw new InvalidOperationException("Role assign failed.");
        }

        public List<ApplicationUser> GetUserList()
        {
            var users = UserManager.Users.ToList();

            return users;
        }
    }
}