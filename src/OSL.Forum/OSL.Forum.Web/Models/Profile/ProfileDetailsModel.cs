﻿using System.Collections.Generic;
using System.Threading.Tasks;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using BO = OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models.Profile
{
    public class ProfileDetailsModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<BO.Post> Posts { get; set; }
        public Pager Pager { get; set; }
        private IProfileService _profileService;
        private IPostService _postService;

        public ProfileDetailsModel()
        {
            _profileService = new ProfileService();
            _postService = new PostService();
        }

        public void GetUserInfo()
        {
            ApplicationUser = _profileService.GetUser();
        }

        public void GetMyPosts(int? page)
        {
            var userTotalPost = _postService.UserPostCount(ApplicationUser.Id);

            Pager = new Pager(userTotalPost, page);

            Posts = _postService.GetMyPosts(Pager.CurrentPage, Pager.PageSize, ApplicationUser.Id);
        }
    }
}