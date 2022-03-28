using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.PendingPost
{
    public class PendingPostListModel
    {
        public List<Post> Posts { get; set; }
        private ILifetimeScope _scope;
        private IPostService _postService;

        public PendingPostListModel()
        {

        }

        public PendingPostListModel(IPostService postService)
        {
            _postService = postService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postService = _scope.Resolve<IPostService>();
        }

        public void GetPendingPostList()
        {
            Posts = _postService.PendingPosts();
        }
    }
}