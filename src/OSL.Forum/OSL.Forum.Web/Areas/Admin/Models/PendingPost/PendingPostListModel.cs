using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
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

        protected override Task Resolve()
        {
            _postService = PostService.Create();
            _profileService = ProfileService.Create();

            return Task.CompletedTask;
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