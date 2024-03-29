﻿using System;
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
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Post
{
    public class EditPostModel : BaseModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Post Title")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public long TopicId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        public BO.Topic Topic { get; set; }
        private DateTime Time { get; set; }
        private ILifetimeScope _scope;
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;
        private IMapper _mapper;

        public EditPostModel()
        {
        }

        public EditPostModel(IMapper mapper, IDateTimeUtility dateTimeUtility, ITopicService topicService,
            IPostService postService)
        {
            _mapper = mapper;
            _dateTimeUtility = dateTimeUtility;
            _topicService = topicService;
            _postService = postService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();
            _postService = _scope.Resolve<IPostService>();
            _scope.Resolve<IForumService>();

            await base.ResolveAsync(_scope);
        }

        public void GetPost(long postId)
        {
            var post = _postService.GetPost(postId);
            _mapper.Map(post, this);
        }

        public void EditPost()
        {
            Time = _dateTimeUtility.Now;

            var post = _mapper.Map<BO.Post>(this);
            post.ModificationDate = Time;
            post.Status = Status.Pending.ToString();

            _postService.EditPost(post);
        }

        public void UpdateTopicModificationDate()
        {
            _topicService.UpdateModificationDate(TopicId, Time);
        }

        public void Delete(long postId)
        {
            _postService.DeletePost(postId);
        }
    }
}