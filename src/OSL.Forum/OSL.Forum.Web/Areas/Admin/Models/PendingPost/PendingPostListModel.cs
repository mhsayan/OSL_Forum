using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.PendingPost
{
    public class PendingPostListModel : BaseModel
    {
        public List<Post> Posts { get; set; }
        public Pager Pager { get; set; }
        private IPostService _postService;
        private IProfileService _profileService;

        public PendingPostListModel()
        {
        }

        public override async Task Resolve()
        {
            _postService = new PostService();
            _profileService = new ProfileService();

            await base.Resolve();
        }

        public void GetPendingPostList(int? page)
        {
            var totalPendingPost = _postService.GetPendingPostCount();

            Pager = new Pager(totalPendingPost, page);
            Posts = _postService.PendingPosts(Pager.CurrentPage, Pager.PageSize);

            foreach (var post in Posts)
            {
                post.OwnerName = _profileService.GetUser(post.ApplicationUserId).Name;
            }
        }

        public void UpdatePostStatus(long postId, string status)
        {
            if (postId == 0)
                throw new ArgumentException("Post Id is required");

            _postService.UpdatePostStatus(postId, status);
        }
    }
}