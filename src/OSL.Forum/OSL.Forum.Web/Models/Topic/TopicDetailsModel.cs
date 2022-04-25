using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Entities;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Topic
{
    public class TopicDetailsModel : BaseModel
    {
        private ApplicationUser ApplicationUser { get; set; }
        public BO.Topic Topic { get; set; }
        private DateTime Time { get; set; }
        public bool IsAuthenticated { get; set; }
        public IList<string> UserRoles { get; set; }
        private ITopicService _topicService;
        private IProfileService _profileService;
        private IMapper _mapper;

        public TopicDetailsModel()
        {
        }

        protected override Task Resolve()
        {
            _profileService = ProfileService.Create();
            _topicService = TopicService.Create();

            return Task.CompletedTask;
        }

        public void GetTopic(long topicId)
        {
            Topic = _topicService.GetTopic(topicId);

            var postList = new List<BO.Post>();

            foreach (var topicPost in Topic.Posts)
            {
                var post = new BO.Post();

                if (topicPost.Status == Status.Approved.ToString())
                {
                    topicPost.Owner = _profileService.Owner(topicPost.ApplicationUserId);
                    topicPost.OwnerName = _profileService.GetUser(topicPost.ApplicationUserId).Name;

                    _mapper.Map(topicPost, post);

                    postList.Add(post);
                }
            }

            Topic.Posts = postList;
        }

        public async Task GetUserRolesAsync()
        {
            UserRoles = await _profileService.UserRolesAsync();
        }

        public void UserAuthenticatedStatus()
        {
            IsAuthenticated = _profileService.IsAuthenticated();
        }
    }
}