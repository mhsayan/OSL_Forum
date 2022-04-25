using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Entities;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models.Profile
{
    public class EditProfileModel : BaseModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private IProfileService _profileService;
        private IMapper _mapper;

        public EditProfileModel()
        {

        }

        protected override Task Resolve()
        {
            _profileService = ProfileService.Create();

            return Task.CompletedTask;
        }

        public async Task EditProfileAsync()
        {
            var applicationUser = _mapper.Map<ApplicationUser>(this);

            await _profileService.EditProfileAsync(applicationUser);
        }

        public void LoadUserInfo()
        {
            var user = _profileService.GetUser();
            _mapper.Map(user, this);
        }
    }
}