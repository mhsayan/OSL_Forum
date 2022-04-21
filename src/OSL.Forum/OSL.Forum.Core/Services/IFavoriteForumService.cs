using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
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
