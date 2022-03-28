using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.PendingPost
{
    public class PendingPostDetailsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ApprovalStatus { get; set; }
        public Guid TopicId { get; set; }
        public Post Post { get; set; }
        private ILifetimeScope _scope;
        private IPostService _postService;
        private IProfileService _profileService;
        private IMapper _mapper;

        public PendingPostDetailsModel()
        {

        }

        public PendingPostDetailsModel(IPostService postService, IMapper mapper,
            IProfileService profileService)
        {
            _postService = postService;
            _profileService = profileService;
            _mapper = mapper;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postService = _scope.Resolve<IPostService>();
            _profileService = _scope.Resolve<IProfileService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public void GetPendingPost(Guid postId)
        {
            Post = _postService.GetPost(postId);
            Post.OwnerName = _profileService.GetUser(Post.ApplicationUserId).Name;

            _mapper.Map(Post, this);
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