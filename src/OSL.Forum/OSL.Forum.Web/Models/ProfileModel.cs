using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class ProfileModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<BO.Post> Posts { get; set; }
        public Pager Pager { get; set; }

        public ProfileModel()
        {
            
        }

        public ProfileModel(ApplicationUser applicationUser)
        {
            FillUpModelData(applicationUser);
        }

        public void FillUpModelData(ApplicationUser applicationUser)
        {
            this.Id = applicationUser.Id;
            this.Name = applicationUser.Name;
        }

        public ApplicationUser UserBuilder()
        {
            var applicationUser = new ApplicationUser()
            {
                Id = this.Id,
                Name = this.Name
            };

            return applicationUser;
        }
    }
}