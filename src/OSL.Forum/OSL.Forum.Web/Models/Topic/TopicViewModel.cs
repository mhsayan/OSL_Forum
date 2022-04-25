using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Enums;
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
        public IList<string> UserRoles { get; set; }
        public Pager Pager { get; set; }
        private IForumService _forumService;
        private ITopicService _topicService;
        private IFavoriteForumService _favoriteForumService;
        private IProfileService _profileService;
        private IMapper _mapper;

        public TopicViewModel()
        {
        }

        protected override Task Resolve()
        {
            _forumService = ForumService.Create();
            _favoriteForumService = FavoriteForumService.Create();
            _profileService = ProfileService.Create();
            _topicService = TopicService.Create();

            return Task.CompletedTask;
        }

        public void GetForum(long forumId)
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

        public void GetTopics(int? page, long forumId)
        {
            var totalItem = _topicService.GetTopicCount(forumId);

            Pager = new Pager(totalItem, page);
            Topics = _topicService.GetTopics(Pager.CurrentPage, Pager.PageSize, forumId);

            var topicList = new List<BO.Topic>();

            foreach (var topic in Topics)
            {
                var topicOb = new BO.Topic();

                topic.Owner = _profileService.Owner(topic.ApplicationUserId);

                _mapper.Map(topic, topicOb);

                topicList.Add(topic);
            }

            Topics = topicList;
        }

        public void Delete(long topicId)
        {
            _topicService.DeleteTopic(topicId);
        }

        public async Task GetUserRolesAsync()
        {
            UserRoles = await _profileService.UserRolesAsync();
        }

        public void Close(long topicId)
        {
            _topicService.CloseTopic(topicId);
        }

        public void Open(long topicId)
        {
            _topicService.OpenTopic(topicId);
        }
    }
}