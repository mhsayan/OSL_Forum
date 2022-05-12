﻿using System;
using System.Collections.Generic;
using OSL.Forum.Base;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public interface IFavoriteForumRepository : IDisposable
    {
        void Save();
        void Add(FavoriteForum favoriteForum);
        void RemoveById(long favoriteForumId);
        FavoriteForum Get(long forumId, string userId);
        IList<FavoriteForum> LoadByUserId(string userId);
        IList<FavoriteForum> Load(string userId, int pageIndex, int pageSize, bool tracking);
    }
}