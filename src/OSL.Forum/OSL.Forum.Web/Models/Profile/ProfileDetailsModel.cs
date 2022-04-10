using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.Membership.Entities;
using OSL.Forum.NHibernate.Core.Utilities;
using OSL.Forum.Membership.Services;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Profile
{
    public class ProfileDetailsModel : BaseModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<BO.Post> Posts { get; set; }
        public Pager Pager { get; set; }
        private IProfileService _profileService { get; set; }
        private IPostService _postService { get; set; }
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ProfileDetailsModel()
        {

        }

        public ProfileDetailsModel(IProfileService profileService, IMapper mapper,
            IPostService postService)
        {
            _profileService = profileService;
            _mapper = mapper;
            _postService = postService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();
            _postService = _scope.Resolve<IPostService>();

            await base.ResolveAsync(_scope);
        }

        public void GetUserInfo()
        {
            ApplicationUser = _profileService.GetUser();
        }

        public void GetMyPosts(int? page)
        {
            var userTotalPost = _postService.UserPostCount(ApplicationUser.Id);

            Pager = new Pager(userTotalPost, page);

            Posts = _postService.GetMyPosts(Pager.CurrentPage, Pager.PageSize, ApplicationUser.Id);
        }
    }
}