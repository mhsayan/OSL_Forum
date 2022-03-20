using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Web.Areas.Admin.Models.Category;

namespace OSL.Forum.Web.Profiles
{
    public class WebProfiles : Profile
    {
        public WebProfiles()
        {
            CreateMap<CreateCategoryModel, Category>().ReverseMap();
        }
    }
}