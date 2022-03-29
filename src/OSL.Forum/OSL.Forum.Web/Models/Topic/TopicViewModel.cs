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
        private ILifetimeScope _scope;
        private IForumService _forumService;
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;
        public TopicViewModel()
        {
        }

        public TopicViewModel(IForumService forumService,
            IFavoriteForumService favoriteForumService, IProfileService profileService)
        {
            _forumService = forumService;
            _favoriteForumService = favoriteForumService;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _forumService = _scope.Resolve<IForumService>();
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
    }
}