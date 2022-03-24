using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class CreateTopicModel
    {
        [Required]
        [Display(Name = "Topic Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public Guid Id { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        private ApplicationUser ApplicationUser { get; set; }
        private DateTime Time { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;
        private IMapper _mapper;

        public CreateTopicModel()
        {
        }

        public CreateTopicModel(ICategoryService categoryService,
            IMapper mapper, IDateTimeUtility dateTimeUtility,
            IProfileService profileService, ITopicService topicService,
            IPostService postService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _dateTimeUtility = dateTimeUtility;
            _profileService = profileService;
            _topicService = topicService;
            _postService = postService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _profileService = _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();
            _postService = _scope.Resolve<IPostService>();
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
                ForumId = this.Id,
                Status = Status.Pending.ToString()
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
    }
}