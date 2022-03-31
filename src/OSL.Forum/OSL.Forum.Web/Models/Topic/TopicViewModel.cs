using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Topic
{
    public class TopicViewModel : BaseModel
    {
        public BO.Forum Forum { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsFavorite { get; set; }
        public IList<BO.Topic> Topics { get; set; }
        public Pager Pager { get; set; }
        private ILifetimeScope _scope;
        private IForumService _forumService;
        private ITopicService _topicService;
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;
        public TopicViewModel()
        {
        }

        public TopicViewModel(IForumService forumService,
            IFavoriteForumService favoriteForumService,
            IProfileService profileService,
            ITopicService topicService)
        {
            _forumService = forumService;
            _favoriteForumService = favoriteForumService;
            _profileService = profileService;
            _topicService = topicService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _forumService = _scope.Resolve<IForumService>();
            _favoriteForumService = _scope.Resolve<IFavoriteForumService>();
            _profileService = _scope.Resolve<IProfileService>();
            _topicService = _scope.Resolve<ITopicService>();

            await base.ResolveAsync(_scope);
        }

        public void GetForum(Guid forumId)
        {
            Forum = _forumService.GetForum(forumId);
        }

        public void LoadUserInfo()
        {
            IsAuthenticated = _profileService.IsAuthenticated();
        }

        public void FavoriteForumStatus()
        {
            var user = _profileService.GetUser();

            var favoriteForum = _favoriteForumService.GetFavoriteForum(Forum.Id, user.Id);

            IsFavorite = favoriteForum != null;
        }

        public void GetTopics(int? page, Guid forumId)
        {
            var totalItem = _topicService.GetTopicCount(forumId);

            Pager = new Pager(totalItem, page);
            Topics = _topicService.GetTopics(Pager.CurrentPage, Pager.PageSize, forumId);
        }
    }
}