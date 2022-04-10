using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.NHibernate.Core.Utilities;
using OSL.Forum.Membership.Entities;
using OSL.Forum.Membership.Services;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Home
{
    public class DetailsModel : BaseModel
    {
        public BO.Category Category { get; set; }
        public IList<string> Roles { get; set; }
        public IList<BO.Forum> Forums { get; set; }
        public Pager Pager { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IForumService _forumService;
        private IMapper _mapper;
        private IProfileService _profileService;

        public DetailsModel()
        {
        }

        public DetailsModel(ICategoryService categoryService,
            IMapper mapper, IForumService forumService, IProfileService profileService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _forumService = forumService;
            _profileService = profileService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _forumService = _scope.Resolve<IForumService>();
            _profileService = _scope.Resolve<IProfileService>();

            await base.ResolveAsync(_scope);
        }

        public void GetCategory(Guid categoryId)
        {
            Category = _categoryService.GetCategory(categoryId);
        }

        public void GetForums(Guid categoryId, int? page)
        {
            var totalItem = _forumService.GetForumCount(categoryId);

            Pager = new Pager(totalItem, page);
            Forums = _forumService.GetForums(Pager.CurrentPage, Pager.PageSize, categoryId);
        }

        public async Task LoadUserInfo()
        {
            Roles = await _profileService.UserRolesAsync();
        }
    }
}