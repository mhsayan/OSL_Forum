using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Topic
{
    public class TopicDetailsModel
    {
        private ApplicationUser ApplicationUser { get; set; }
        public BO.Forum Forum { get; set; }
        public BO.Category Category { get; set; }
        public BO.Topic Topic { get; set; }
        private DateTime Time { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private ITopicService _topicService;
        private IProfileService _profileService;
        private IForumService _forumService;
        private IMapper _mapper;

        public TopicDetailsModel()
        {
        }

        public TopicDetailsModel(ICategoryService categoryService,
            IMapper mapper, IProfileService profileService,
            ITopicService topicService, IForumService forumService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _profileService = profileService;
            _topicService = topicService;
            _forumService = forumService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _profileService = _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();
            _forumService = _scope.Resolve<IForumService>();
        }

        public void GetTopic(Guid topicId)
        {
            Topic = _topicService.GetTopic(topicId);
        }

        public void GetCategory()
        {
            Category = _categoryService.GetCategory(Forum.CategoryId);
        }

        public void GetForum()
        {
            Forum = _forumService.GetForum(Topic.ForumId);
        }
    }
}