using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EO = OSL.Forum.NHibernate.Core.Entities;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.NHibernate.Core.Profiles
{
    public class NHibernateCoreProfiles : Profile
    {
        public NHibernateCoreProfiles()
        {
            CreateMap<EO.Category, BO.Category>().ReverseMap();
            CreateMap<EO.Forum, BO.Forum>().ReverseMap();
            CreateMap<EO.Topic, BO.Topic>().ReverseMap();
            CreateMap<EO.Post, BO.Post>().ReverseMap();
            CreateMap<EO.FavoriteForum, BO.FavoriteForum>().ReverseMap();
            CreateMap<BO.ApplicationUserRole, BO.ApplicationUserRole>().ReverseMap();
            CreateMap<EO.FavoriteForum, BO.FavoriteForum>().ReverseMap();
        }
    }
}
