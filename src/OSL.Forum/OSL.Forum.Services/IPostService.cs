using System.Collections.Generic;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Services
{
    public interface IPostService
    {
        BO.Post GetPost(long postId);
        void EditPost(BO.Post post);
        void DeletePost(long postId);
        void CreatePost(BO.Post post);
        List<BO.Post> PendingPosts(int pagerCurrentPage, int pagerPageSize);
        void UpdatePostStatus(long postId, string status);
        int UserPostCount(string applicationUserId);
        List<BO.Post> GetMyPosts(int pagerCurrentPage, int pagerPageSize, string applicationUserId);
        int GetPendingPostCount();
    }
}
