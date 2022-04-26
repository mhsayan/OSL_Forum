using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
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

        public TopicDetailsModel()
        {
        }

        public override async Task Resolve()
        {
            _profileService = ProfileService.Create();
            _topicService = TopicService.Create();

            await base.Resolve();
        }

        public void GetTopic(long topicId)
        {
            Topic = _topicService.GetTopic(topicId);

            var postList = new List<BO.Post>();

            foreach (var topicPost in Topic.Posts)
            {
                if (topicPost.Status == Status.Approved.ToString())
                {
                    topicPost.Owner = _profileService.Owner(topicPost.ApplicationUserId);
                    topicPost.OwnerName = _profileService.GetUser(topicPost.ApplicationUserId).Name;

                    var post = new BO.Post()
                    {
                        Id = topicPost.Id,
                        Name = topicPost.Name,
                        TopicId = topicPost.TopicId,
                        ApplicationUserId = topicPost.ApplicationUserId,
                        Owner = topicPost.Owner,
                        OwnerName = topicPost.OwnerName,
                        Description = topicPost.Description,
                        Status = topicPost.Status,
                        CreationDate = topicPost.CreationDate,
                        ModificationDate = topicPost.ModificationDate,
                        Topic = topicPost.Topic
                    };

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