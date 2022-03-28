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

namespace OSL.Forum.Web.Models.Post
{
    public class CreatePostModel
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
        public BO.Forum BoForum { get; set; }
        public BO.Category BoCategory { get; set; }
        private DateTime Time { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;
        private IForumService _forumService;
        private IMapper _mapper;

        public CreatePostModel()
        {
        }

        public CreatePostModel(ICategoryService categoryService,
            IMapper mapper, IDateTimeUtility dateTimeUtility,
            IProfileService profileService, ITopicService topicService,
            IPostService postService, IForumService forumService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _dateTimeUtility = dateTimeUtility;
            _profileService = profileService;
            _topicService = topicService;
            _postService = postService;
            _forumService = forumService;
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
            _forumService = _scope.Resolve<IForumService>();
        }

        public void CreatePost()
        {
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

        public void GetCategory()
        {
            BoCategory = _categoryService.GetCategory(BoForum.CategoryId);
        }

        public void GetForum()
        {
            BoForum = _forumService.GetForum(BoTopic.ForumId);
        }
    }
}