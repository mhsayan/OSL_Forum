﻿using System.Configuration;
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

            if (!roleManager.RoleExists(Roles.SuperAdmin.ToString()))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.SuperAdmin.ToString()));
            }

            if (!roleManager.RoleExists(Roles.Admin.ToString()))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.Admin.ToString()));
            }

            if (!roleManager.RoleExists(Roles.Moderator.ToString()))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.Moderator.ToString()));
            }

            if (!roleManager.RoleExists(Roles.User.ToString()))
            {
                var roleResult = roleManager.Create(new IdentityRole(Roles.User.ToString()));
            }

            string name = ConfigurationManager.AppSettings["Name"].ToString();
            string userName = ConfigurationManager.AppSettings["SuperAdminEmail"].ToString();
            string password = ConfigurationManager.AppSettings["SuperAdminPassword"].ToString();

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
                    var result = userManager.AddToRole(user.Id, Roles.SuperAdmin.ToString());
                }
            }
        }
    }
}