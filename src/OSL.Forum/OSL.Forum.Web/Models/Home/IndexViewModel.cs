using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Entities;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Home
{
    public class IndexViewModel : BaseModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public IList<BO.Category> Categories { get; set; }
        public bool IsAuthenticated { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;

        public IndexViewModel()
        {
        }

        public IndexViewModel(ICategoryService categoryService,
            IFavoriteForumService favoriteForumService,
            IProfileService profileService)
        {
            _categoryService = categoryService;
            _favoriteForumService = favoriteForumService;
            _profileService = profileService;
        }

        public override async Task Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _favoriteForumService = _scope.Resolve<IFavoriteForumService>();
            _profileService = _scope.Resolve<IProfileService>();

            await base.Resolve(_scope);
        }

        public void GetCategories()
        {
            Categories = _categoryService.GetCategories();
        }

        public void UserAuthenticatedStatus()
        {
            IsAuthenticated = _profileService.IsAuthenticated();
        }

        public void GetFavoriteForums()
        {
            var user = _profileService.GetUser();
            FavoriteForums = _favoriteForumService.GetUserFavoriteForums(user.Id).Take(4).ToList();
        }
    }
}