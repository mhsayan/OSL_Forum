using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Membership.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Post
{
    public class CreatePostModel : BaseModel
    {
        [Required]
        [Display(Name = "Post Title")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText), AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        public Guid TopicId { get; set; }
        public BO.Topic BoTopic { get; set; }
        private DateTime Time { get; set; }
        private ILifetimeScope _scope;
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;
        private IMapper _mapper;

        public CreatePostModel()
        {
        }

        public CreatePostModel(IMapper mapper, IDateTimeUtility dateTimeUtility,
            IProfileService profileService, ITopicService topicService,
            IPostService postService)
        {
            _mapper = mapper;
            _dateTimeUtility = dateTimeUtility;
            _profileService = profileService;
            _topicService = topicService;
            _postService = postService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _mapper = _scope.Resolve<IMapper>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _profileService = _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();
            _postService = _scope.Resolve<IPostService>();

            await base.ResolveAsync(_scope);
        }

        public void CreatePost()
        {
            if (BoTopic.ActivityStatus != ActivityStatus.Active.ToString())
                return;

            var user = _profileService.GetUser();
            Time = _dateTimeUtility.Now;

            var post = _mapper.Map<BO.Post>(this);
            post.CreationDate = Time;
            post.ModificationDate = Time;
            post.ApplicationUserId = user.Id;

            if (BoTopic.ApprovalType == ApprovalType.Auto.ToString())
                post.Status = Status.Approved.ToString();
            else post.Status = Status.Pending.ToString();

            _postService.CreatePost(post);
        }

        public void UpdateTopicModificationDate()
        {
            _topicService.UpdateModificationDate(TopicId, Time);
        }

        public void GetTopic(Guid topicId)
        {
            BoTopic = _topicService.GetTopic(topicId);
        }
    }
}