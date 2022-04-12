using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OSL.Forum.Membership.Services;
using OSL.Forum.NHibernate.Core.UnitOfWorks;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;
using EO = OSL.Forum.NHibernate.Core.Entities;

namespace OSL.Forum.NHibernate.Core.Services
{
    public class FavoriteForumService : IFavoriteForumService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IProfileService _profileService;
        private IMapper _mapper;

        public FavoriteForumService(ICoreUnitOfWork unitOfWork,
            IMapper mapper, IProfileService profileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _profileService = profileService;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(string userId)
        {
            var favoriteForumsEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ApplicationUserId == userId);

            var favoriteForums = favoriteForumsEntity.Select(favoriteForum =>
                _mapper.Map<BO.FavoriteForum>(favoriteForum)
            ).ToList();

            return favoriteForums;
        }

        public List<BO.FavoriteForum> GetUserFavoriteForums(int pageIndex, int pageSize, string userId)
        {
            var favoriteForumsEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ApplicationUserId == userId, q => q.OrderBy(c => c.ForumId), pageIndex, pageSize);

            var favoriteForums = favoriteForumsEntity.Select(favoriteForum =>
                _mapper.Map<BO.FavoriteForum>(favoriteForum)
                ).ToList();

            return favoriteForums;
        }

        public int GetFavoriteForumCount(string userId)
        {
            return _unitOfWork.FavoriteForums.Get(ff => ff.ApplicationUserId == userId).Count;
        }

        public BO.FavoriteForum GetFavoriteForum(Guid forumId, string userId)
        {
            var favoriteForumEntity = _unitOfWork.FavoriteForums
                .Get(ff => ff.ForumId == forumId && ff.ApplicationUserId == userId).FirstOrDefault();

            if (favoriteForumEntity == null)
                return null;

            var favoriteForum = _mapper.Map<BO.FavoriteForum>(favoriteForumEntity);

            return favoriteForum;
        }

        public void AddToFavorite(Guid forumId, string userId)
        {
            var oldFavoriteForum = GetFavoriteForum(forumId, userId);

            if (oldFavoriteForum != null)
                throw new InvalidOperationException("This Forum is in your Favorite list.");

            var favoriteForumEntity = new EO.FavoriteForum()
            {
                Forum = _unitOfWork.Forums.GetById(forumId),
                ApplicationUser = _profileService.GetUser(userId)
            };

            _unitOfWork.FavoriteForums.Add(favoriteForumEntity);
            _unitOfWork.Save();
        }

        public void RemoveFromFavorite(Guid forumId, string userId)
        {
            var oldFavoriteForum = GetFavoriteForum(forumId, userId);

            if (oldFavoriteForum == null)
                throw new InvalidOperationException("This Forum is not in your Favorite list.");

            _unitOfWork.FavoriteForums.Remove(oldFavoriteForum.Id);
            _unitOfWork.Save();
        }
    }
}
