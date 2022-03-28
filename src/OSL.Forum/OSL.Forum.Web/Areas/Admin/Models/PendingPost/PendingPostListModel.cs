using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.PendingPost
{
    public class PendingPostListModel
    {
        public List<Post> Posts { get; set; }
        private ILifetimeScope _scope;
        private IPostService _postService;
        private IProfileService _profileService;

        public PendingPostListModel()
        {

        }

        public PendingPostListModel(IPostService postService, IProfileService profileService)
        {
            _postService = postService;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postService = _scope.Resolve<IPostService>();
            _profileService = _scope.Resolve<IProfileService>();
        }

        public void GetPendingPostList()
        {
            Posts = _postService.PendingPosts();

            foreach (var post in Posts)
            {
                post.OwnerName = _profileService.GetUser(post.ApplicationUserId).Name;
            }
        }

        public void AcceptPost(Guid postId)
        {
            if (postId == Guid.Empty)
                throw new ArgumentException(nameof(postId));

            _postService.AcceptPost(postId);
        }

        public void RejectPost(Guid postId)
        {
            if (postId == Guid.Empty)
                throw new ArgumentException(nameof(postId));

            _postService.RejectPost(postId);
        }
    }
}