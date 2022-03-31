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
        void AddToFavorite(Guid forumId, string userId);
        void RemoveFromFavorite(Guid forumId, string userId);
        BO.FavoriteForum GetFavoriteForum(Guid forumId, string userId);
        List<BO.FavoriteForum> GetUserFavoriteForums(int pageIndex, int pageSize, string userId);
        List<BO.FavoriteForum> GetUserFavoriteForums(string userId);
        int GetFavoriteForumCount(string userId);
    }
}
