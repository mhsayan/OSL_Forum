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
        private IProfileService _profileService { get; set; }
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public EditProfileModel()
        {

        }

        public EditProfileModel(IProfileService profileService, IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();

            await base.ResolveAsync(_scope);
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