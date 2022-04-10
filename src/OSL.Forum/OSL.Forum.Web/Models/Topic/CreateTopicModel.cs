using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernate.Core.Enums;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.NHibernate.Core.Utilities;
using OSL.Forum.Membership.Entities;
using OSL.Forum.Membership.Services;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Topic
{
    public class CreateTopicModel : BaseModel
    {
        [Required]
        [Display(Name = "Topic Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public Guid ForumId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        private ApplicationUser ApplicationUser { get; set; }
        public BO.Forum Forum { get; set; }
        private DateTime Time { get; set; }
        private ILifetimeScope _scope;
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;
        private IForumService _forumService;

        public CreateTopicModel()
        {
        }

        public CreateTopicModel(IDateTimeUtility dateTimeUtility,
            IProfileService profileService, ITopicService topicService,
            IPostService postService, IForumService forumService)
        {
            _dateTimeUtility = dateTimeUtility;
            _profileService = profileService;
            _topicService = topicService;
            _postService = postService;
            _forumService = forumService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _profileService = _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();
            _postService = _scope.Resolve<IPostService>();
            _forumService = _scope.Resolve<IForumService>();

            await base.ResolveAsync(_scope);
        }

        public void Create()
        {
            ApplicationUser = _profileService.GetUser();

            if (ApplicationUser == null)
                throw new InvalidOperationException(nameof(ApplicationUser));

            Time = _dateTimeUtility.Now;

            var topic = new BO.Topic()
            {
                Name = this.Name,
                CreationDate = Time,
                ModificationDate = Time,
                ApplicationUserId = ApplicationUser.Id,
                ForumId = this.ForumId,
                Status = Status.Pending.ToString(),
                ApprovalType = ApprovalType.Manual.ToString(),
                ActivityStatus = ActivityStatus.Inactive.ToString()
            };

            _topicService.CreateTopic(topic);
        }

        public void CreatePost()
        {
            var topic = _topicService.GetTopic(Name);

            if (topic == null)
                throw new InvalidOperationException("Topic is missing.");

            var post = new BO.Post()
            {
                Name = this.Name,
                Description = this.Description,
                CreationDate = Time,
                ModificationDate = Time,
                ApplicationUserId = ApplicationUser.Id,
                TopicId = topic.Id,
                Status = Status.Pending.ToString()
            };

            _postService.CreatePost(post);
        }

        public void GetForum(Guid forumId)
        {
            Forum = _forumService.GetForum(forumId);
        }
    }
}