using System;
using System.Collections.Generic;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.NHibernate.Core.Services
{
    public interface IFavoriteForumService
    {
        void AddToFavorite(Guid forumId, string userId);
        void RemoveFromFavorite(Guid forumId, string userId);
        BO.FavoriteForum GetFavoriteForum(Guid forumId, string userId);
        List<BO.FavoriteForum> GetUserFavoriteForums(int pageIndex, int pageSize, string userId);
        List<BO.FavoriteForum> GetUserFavoriteForums(string userId);
        int GetFavoriteForumCount(string userId);
    }
}
