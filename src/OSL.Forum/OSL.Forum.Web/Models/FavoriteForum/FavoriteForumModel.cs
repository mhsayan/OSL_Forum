using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models.FavoriteForum
{
    public class FavoriteForumModel
    {
        private IFavoriteForumService _favoriteForumService;
        private IForumService _forumService;
        private IMapper _mapper;
        private IProfileService _profileService;
        private ILifetimeScope _scope;

        public FavoriteForumModel()
        {

        }

        public FavoriteForumModel(IFavoriteForumService favoriteForumService,
            IForumService forumService, IMapper mapper, IProfileService profileService)
        {
            _favoriteForumService = favoriteForumService;
            _forumService = forumService;
            _mapper = mapper;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();
            _favoriteForumService = _scope.Resolve<IFavoriteForumService>();
            _forumService = _scope.Resolve<IForumService>();
        }

        public void AddToFavorite(Guid forumId)
        {
            var user = _profileService.GetUser();

            _favoriteForumService.AddToFavorite(forumId, user.Id);
        }

        public void RemoveFromFavorite(Guid forumId)
        {
            var user = _profileService.GetUser();

            _favoriteForumService.RemoveFromFavorite(forumId, user.Id);
        }
    }
}