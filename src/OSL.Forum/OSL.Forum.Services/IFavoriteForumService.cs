using System.Collections.Generic;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Services
{
    public interface IFavoriteForumService
    {
        void AddToFavorite(long forumId, string userId);
        void RemoveFromFavorite(long forumId, string userId);
        BO.FavoriteForum GetFavoriteForum(long forumId, string userId);
        List<BO.FavoriteForum> GetUserFavoriteForums(int pageIndex, int pageSize, string userId);
        List<BO.FavoriteForum> GetUserFavoriteForums(string userId);
        int GetFavoriteForumCount(string userId);
    }
}
