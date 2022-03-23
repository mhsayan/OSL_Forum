using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Areas.Admin.Models.Category;
using OSL.Forum.Web.Areas.Admin.Models.Forum;
using OSL.Forum.Web.Models.Home;

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
        }
    }
}