using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Membership.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.FavoriteForum
{
    public class FavoriteForumModel : BaseModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public Pager Pager { get; set; }
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

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();
            _favoriteForumService = _scope.Resolve<IFavoriteForumService>();
            _forumService = _scope.Resolve<IForumService>();

            await base.ResolveAsync(_scope);
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

        public void GetFavoriteForums(int? page)
        {
            var user = _profileService.GetUser();
            var totalItem = _favoriteForumService.GetFavoriteForumCount(user.Id);

            Pager = new Pager(totalItem, page);
            FavoriteForums = _favoriteForumService.GetUserFavoriteForums(Pager.CurrentPage, Pager.PageSize, user.Id);
        }
    }
}