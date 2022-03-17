using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Seeds
{
    public class IdentityHelper
    {
        internal static void SeedIdentities(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(Roles.SuperAdmin))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.SuperAdmin));
            }

            if (!roleManager.RoleExists(Roles.Admin))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.Admin));
            }

            if (!roleManager.RoleExists(Roles.Moderator))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.Moderator));
            }

            if (!roleManager.RoleExists(Roles.User))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.User));
            }

            string name = "Super Admin";
            string userName = "superadmin@oslforum.com";
            string password = "Superadmin@1234#";

            var user = userManager.FindByName(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Name = name,
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };

                IdentityResult userResult = userManager.Create(user, password);

                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, Roles.SuperAdmin);
                }
            }
        }
    }
}