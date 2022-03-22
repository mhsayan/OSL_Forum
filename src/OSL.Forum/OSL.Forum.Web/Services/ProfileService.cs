using System;
using System.Collections.Generic;
using System.Linq;
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
            var user = UserManager.FindByName(userId);

            return user ?? null;
        }
    }
}