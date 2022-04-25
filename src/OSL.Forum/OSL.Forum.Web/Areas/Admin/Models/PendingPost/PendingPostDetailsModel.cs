using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.PendingPost
{
    public class PendingPostDetailsModel : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ApprovalStatus { get; set; }
        public long TopicId { get; set; }
        public Post Post { get; set; }
        private IPostService _postService;
        private IProfileService _profileService;
        private ITopicService _topicService;
        private IMapper _mapper;

        public PendingPostDetailsModel()
        {
        }

        public override Task Resolve()
        {
            _postService = PostService.Create();
            _profileService = ProfileService.Create();
            _topicService = TopicService.Create();

            return Task.CompletedTask;
        }

        public void GetPendingPost(long postId)
        {
            Post = _postService.GetPost(postId);
            Post.OwnerName = _profileService.GetUser(Post.ApplicationUserId).Name;

            _mapper.Map(Post, this);
        }

        public void UpdatePostStatus(string status)
        {
            _postService.UpdatePostStatus(Id, status);
        }

        public void UpdateTopicApprovalType()
        {
            if (ApprovalStatus)
                _topicService.UpdateTopicApprovalType(TopicId);
        }
    }
}