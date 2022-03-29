using System;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Topic
{
    public class TopicViewModel
    {
        public BO.Forum Forum { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsFavorite { get; set; }
        public BO.Category Category { get; set; }
        private ILifetimeScope _scope;
        private IForumService _forumService;
        private ICategoryService _categoryService;
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;
        private IMapper _mapper;
        public TopicViewModel()
        {
        }

        public TopicViewModel(IForumService forumService,
            IMapper mapper, ICategoryService categoryService,
            IFavoriteForumService favoriteForumService, IProfileService profileService)
        {
            _forumService = forumService;
            _mapper = mapper;
            _categoryService = categoryService;
            _favoriteForumService = favoriteForumService;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _forumService = _scope.Resolve<IForumService>();
            _mapper = _scope.Resolve<IMapper>();
            _categoryService = _scope.Resolve<ICategoryService>();
            _favoriteForumService = _scope.Resolve<IFavoriteForumService>();
            _profileService = _scope.Resolve<IProfileService>();
        }

        public void GetForum(Guid forumId)
        {
            Forum = _forumService.GetForum(forumId);
        }

        public void LoadUserInfo()
        {
            IsAuthenticated = _profileService.IsAuthenticated();
        }

        public void FavoriteForumStatus()
        {
            var user = _profileService.GetUser();

            var favoriteForum = _favoriteForumService.GetFavoriteForum(Forum.Id, user.Id);

            IsFavorite = favoriteForum != null;
        }

        public void GetCategory()
        {
            Category = _categoryService.GetCategory(Forum.CategoryId);
        }
    }
}