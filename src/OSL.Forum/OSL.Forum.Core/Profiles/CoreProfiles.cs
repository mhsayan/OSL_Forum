using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EO = OSL.Forum.Core.Entities;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Core.Profiles
{
    public class CoreProfiles : Profile
    {
        public CoreProfiles()
        {
            CreateMap<EO.Category, BO.Category>().ReverseMap();
            CreateMap<EO.Forum, BO.Forum>().ReverseMap();
            CreateMap<EO.Topic, BO.Topic>().ReverseMap();
            CreateMap<EO.Post, BO.Post>().ReverseMap();
            CreateMap<EO.FavoriteForum, BO.FavoriteForum>().ReverseMap();
            CreateMap<BO.ApplicationUserRole, BO.ApplicationUserRole>().ReverseMap();
        }
    }
}
