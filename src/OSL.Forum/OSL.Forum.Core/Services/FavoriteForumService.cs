﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class FavoriteForumService : IFavoriteForumService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public FavoriteForumService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(string userId)
        {
            var favoriteForumsEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ApplicationUserId == userId, "");

            var favoriteForums = favoriteForumsEntity.Select(favoriteForum =>
                _mapper.Map<BO.FavoriteForum>(favoriteForum)
            ).ToList();

            return favoriteForums;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(int pageIndex, int pageSize, string userId)
        {
            var favoriteForumsEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ApplicationUserId == userId, q => q.OrderBy(c => c.ForumId), "", pageIndex, pageSize, false);

            var favoriteForums = favoriteForumsEntity.data.Select(favoriteForum =>
                _mapper.Map<BO.FavoriteForum>(favoriteForum)
                ).ToList();

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

            var favoriteForum = _mapper.Map<BO.FavoriteForum>(favoriteForumEntity);

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
