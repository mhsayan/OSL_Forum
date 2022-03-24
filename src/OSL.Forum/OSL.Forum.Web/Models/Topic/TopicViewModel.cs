using System;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Topic
{
    public class TopicViewModel
    {
        public BO.Forum Forum { get; set; }
        public bool IsAuthenticated { get; set; }
        public BO.Category Category { get; set; }
        private ILifetimeScope _scope;
        private IForumService _forumService;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        public TopicViewModel()
        {
        }

        public TopicViewModel(IForumService forumService,
            IMapper mapper, ICategoryService categoryService)
        {
            _forumService = forumService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _forumService = _scope.Resolve<IForumService>();
            _mapper = _scope.Resolve<IMapper>();
            _categoryService = _scope.Resolve<ICategoryService>();
        }

        public void GetForum(Guid forumId)
        {
            Forum = _forumService.GetForum(forumId);
        }

        public void LoadUserInfo()
        {
            IsAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public void GetCategory()
        {
            Category = _categoryService.GetCategory(Forum.CategoryId);
        }
    }
}