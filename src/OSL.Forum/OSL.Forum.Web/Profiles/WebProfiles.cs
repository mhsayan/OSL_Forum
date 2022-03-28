using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Areas.Admin.Models.Category;
using OSL.Forum.Web.Areas.Admin.Models.Forum;
using OSL.Forum.Web.Areas.Admin.Models.PendingPost;
using OSL.Forum.Web.Areas.Admin.Models.UserManagement;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Models.Home;
using OSL.Forum.Web.Models.Post;
using OSL.Forum.Web.Models.Profile;
using IndexViewModel = OSL.Forum.Web.Models.Home.IndexViewModel;

namespace OSL.Forum.Web.Profiles
{
    public class WebProfiles : Profile
    {
        public WebProfiles()
        {
            CreateMap<CreateCategoryModel, Category>().ReverseMap();
            CreateMap<CategoriesModel, Category>().ReverseMap();
            CreateMap<EditCategoryModel, Category>().ReverseMap();
            CreateMap<EditForumModel, Core.BusinessObjects.Forum>().ReverseMap();
            CreateMap<IndexViewModel, Category>().ReverseMap();
            CreateMap<DetailsModel, Category>().ReverseMap();
            CreateMap<EditPostModel, Post>().ReverseMap();
            CreateMap<CreatePostModel, Post>().ReverseMap();
            CreateMap<ProfileDetailsModel, Post>().ReverseMap();
            CreateMap<EditProfileModel, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUser>().ReverseMap();
            CreateMap<AssignRoleModel, ApplicationUserRole>().ReverseMap();
            CreateMap<Post, Post>().ReverseMap();
            CreateMap<PendingPostDetailsModel, Post>().ReverseMap();
        }
    }
}