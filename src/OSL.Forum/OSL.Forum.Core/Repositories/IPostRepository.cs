using System;
using System.Collections;
using System.Collections.Generic;
using OSL.Forum.Base;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public interface IPostRepository : IDisposable
    {
        void Save();
        Post GetById(long topicId);
        void RemoveById(long postId);
        void Add(Post post);
        IList<Post> LoadByStatus(string status);
        IList<Post> LoadByUserId(string applicationUserId);
        IList<Post> LoadPendingPosts(string status, int pagerCurrentPage, int pagerPageSize, bool tracking);
        IList<Post> LoadUserPosts(string userId, int pagerCurrentPage, int pagerPageSize, bool tracking);
    }
}