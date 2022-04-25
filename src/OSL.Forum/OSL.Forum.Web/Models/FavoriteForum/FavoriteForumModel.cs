using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.FavoriteForum
{
    public class FavoriteForumModel : BaseModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public Pager Pager { get; set; }
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;

        public FavoriteForumModel()
        {

        }

        protected override Task Resolve()
        {
            _favoriteForumService = FavoriteForumService.Create();
            _profileService = ProfileService.Create();

            return Task.CompletedTask;
        }

        public void AddToFavorite(long forumId)
        {
            var user = _profileService.GetUser();

            _favoriteForumService.AddToFavorite(forumId, user.Id);
        }

        public void RemoveFromFavorite(long forumId)
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