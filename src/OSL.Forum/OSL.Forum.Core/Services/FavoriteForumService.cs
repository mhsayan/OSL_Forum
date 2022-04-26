using System;
using System.Collections.Generic;
using System.Linq;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class FavoriteForumService : IFavoriteForumService
    {
        private static FavoriteForumService _favoriteForumService;
        private readonly CoreUnitOfWork _unitOfWork;

        private FavoriteForumService()
        {
            _unitOfWork = CoreUnitOfWork.Create();
        }

        public static FavoriteForumService Create()
        {
            if (_favoriteForumService == null)
            {
                _favoriteForumService = new FavoriteForumService();
            }

            return _favoriteForumService;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(string userId)
        {
            var favoriteForumsEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ApplicationUserId == userId, "");

            var favoriteForums = favoriteForumsEntity.Select(favoriteForum =>
                new BO.FavoriteForum
                {
                    Id = favoriteForum.Id,
                    ApplicationUserId = favoriteForum.ApplicationUserId,
                    ForumId = favoriteForum.ForumId,
                    Forum = new BO.Forum()
                    {
                        Id = favoriteForum.Forum.Id,
                        Name = favoriteForum.Forum.Name,
                        CreationDate = favoriteForum.Forum.CreationDate,
                        ModificationDate = favoriteForum.Forum.ModificationDate,
                        CategoryId = favoriteForum.Forum.CategoryId
                    }
                }).ToList();

            return favoriteForums;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(int pageIndex, int pageSize, string userId)
        {
            var favoriteForumsEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ApplicationUserId == userId, q => q.OrderBy(c => c.ForumId), "", pageIndex, pageSize, false);

            var favoriteForums = favoriteForumsEntity.data.Select(favoriteForum =>
                new BO.FavoriteForum
                {
                    Id = favoriteForum.Id,
                    ApplicationUserId = favoriteForum.ApplicationUserId,
                    ForumId = favoriteForum.ForumId,
                    Forum = new BO.Forum()
                    {
                        Id = favoriteForum.Forum.Id,
                        Name = favoriteForum.Forum.Name,
                        CreationDate = favoriteForum.Forum.CreationDate,
                        ModificationDate = favoriteForum.Forum.ModificationDate,
                        CategoryId = favoriteForum.Forum.CategoryId
                    },
                }).ToList();

            return favoriteForums;
        }

        public int GetFavoriteForumCount(string userId)
        {
            return _unitOfWork.FavoriteForums.Get(ff => ff.ApplicationUserId == userId, "").Count;
        }

        public BO.FavoriteForum GetFavoriteForum(long forumId, string userId)
        {
            var favoriteForumEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ForumId == forumId && ff.ApplicationUserId == userId, "").FirstOrDefault();

            if (favoriteForumEntity == null)
                return null;

            var favoriteForum = new BO.FavoriteForum()
            {
                Id = favoriteForumEntity.Id,
                ApplicationUserId = favoriteForumEntity.ApplicationUserId,
                ForumId = favoriteForumEntity.ForumId,
                Forum = new BO.Forum()
                {
                    Id = favoriteForumEntity.Forum.Id,
                    Name = favoriteForumEntity.Forum.Name,
                    CreationDate = favoriteForumEntity.Forum.CreationDate,
                    ModificationDate = favoriteForumEntity.Forum.ModificationDate,
                    CategoryId = favoriteForumEntity.Forum.CategoryId
                }
            };

            return favoriteForum;
        }

        public void AddToFavorite(long forumId, string userId)
        {
            var oldFavoriteForum = GetFavoriteForum(forumId, userId);

            if (oldFavoriteForum != null)
                throw new InvalidOperationException("This Forum is in your Favorite list.");

            var favoriteForumEntity = new EO.FavoriteForum()
            {
                ForumId = forumId,
                ApplicationUserId = userId
            };

            _unitOfWork.FavoriteForums.Add(favoriteForumEntity);
            _unitOfWork.Save();
        }

        public void RemoveFromFavorite(long forumId, string userId)
        {
            var oldFavoriteForum = GetFavoriteForum(forumId, userId);

            if (oldFavoriteForum == null)
                throw new InvalidOperationException("This Forum is not in your Favorite list.");

            _unitOfWork.FavoriteForums.Remove(oldFavoriteForum.Id);
            _unitOfWork.Save();
        }
    }
}
