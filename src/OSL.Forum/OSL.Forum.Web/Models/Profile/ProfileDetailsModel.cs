using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models.Profile
{
    public class ProfileDetailsModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        private IProfileService _profileService { get; set; }
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ProfileDetailsModel()
        {

        }

        public ProfileDetailsModel(IProfileService profileService, IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public void GetUserInfo()
        {
            ApplicationUser = _profileService.GetUser();
        }
    }
}