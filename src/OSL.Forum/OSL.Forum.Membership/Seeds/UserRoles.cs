using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NHibernate.AspNet.Identity;
using OSL.Forum.Membership.Contexts;
using OSL.Forum.Membership.Utilities;

namespace OSL.Forum.Membership.Seeds
{
    public class UserRoles : IdentityRole
    {
        public virtual void GenerateUserRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(MembershipDbContext.GetSession()));

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
        }
    }
}
