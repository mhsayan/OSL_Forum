using System;
using System.Collections.Generic;
using System.Linq;
using OSL.Forum.DAO;
using OSL.Forum.Entities.Contexts;
using BO = OSL.Forum.Entities.BusinessObjects;
using EO = OSL.Forum.Entities;

namespace OSL.Forum.Services
{
    public class FavoriteForumService : IFavoriteForumService
    {
        private readonly IFavoriteForumRepository _favoriteForumRepository;

        public FavoriteForumService()
        {
            _favoriteForumRepository = new FavoriteForumRepository(new CoreDbContext());
        }

        public FavoriteForumService(IFavoriteForumRepository favoriteForumRepository)
        {
            _favoriteForumRepository = favoriteForumRepository;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(string userId)
        {
            var favoriteForumsEntity = _favoriteForumRepository.LoadByUserId(userId);

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
            var favoriteForumsEntity = _favoriteForumRepository.Load(userId, pageIndex, pageSize, false);

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
                    },
                }).ToList();

            return favoriteForums;
        }

        public int GetFavoriteForumCount(string userId)
        {
            return _favoriteForumRepository.LoadByUserId(userId).Count;
        }

        public BO.FavoriteForum GetFavoriteForum(long forumId, string userId)
        {
            var favoriteForumEntity = _favoriteForumRepository.Get(forumId, userId);

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

            _favoriteForumRepository.Add(favoriteForumEntity);
            _favoriteForumRepository.Save();
        }

        public void RemoveFromFavorite(long forumId, string userId)
        {
            var oldFavoriteForum = GetFavoriteForum(forumId, userId);

            if (oldFavoriteForum == null)
                throw new InvalidOperationException("This Forum is not in your Favorite list.");

            _favoriteForumRepository.RemoveById(oldFavoriteForum.Id);
            _favoriteForumRepository.Save();
        }
    }
}
