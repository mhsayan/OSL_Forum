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
using OSL.Forum.Membership.Entities;
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
        private ILifetimeScope _scope;
        private ITopicService _topicService;
        private IProfileService _profileService;
        private IMapper _mapper;

        public TopicDetailsModel()
        {
        }

        public TopicDetailsModel(IMapper mapper,
            IProfileService profileService,
            ITopicService topicService)
        {
            _mapper = mapper;
            _profileService = profileService;
            _topicService = topicService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _mapper = _scope.Resolve<IMapper>();
            _profileService = _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();

            await base.ResolveAsync(_scope);
        }

        public void GetTopic(Guid topicId)
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