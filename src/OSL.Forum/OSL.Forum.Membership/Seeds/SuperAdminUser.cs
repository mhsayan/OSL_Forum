using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OSL.Forum.Membership.Entities;
using OSL.Forum.Membership.Utilities;

namespace OSL.Forum.Membership.Seeds
{
    public class SuperAdminUser
    {
        private readonly UserManager<ApplicationUser> _manager;

        public SuperAdminUser(UserManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        public void GenerateSuperAdminUser()
        {
            var admin = _manager.FindByEmail("admin@demo.com");

            string name = ConfigurationManager.AppSettings["Name"].ToString();
            string email = ConfigurationManager.AppSettings["SuperAdminEmail"].ToString();
            string password = ConfigurationManager.AppSettings["Password"].ToString();

            var user = _manager.FindByEmail(email);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Name = name,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var userResult = _manager.Create(user, password);

                if (userResult.Succeeded)
                {
                    var result = _manager.AddToRole(user.Id, Roles.SuperAdmin.ToString());
                }
            }
        }
    }
}
