using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Profiles;
using OSL.Forum.Web.Areas.Admin.Models.Category;
using OSL.Forum.Web.Areas.Admin.Models.Forum;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Models.Home;
using OSL.Forum.Web.Models.Topic;
using OSL.Forum.Web.Profiles;
using OSL.Forum.Web.Services;
using IndexViewModel = OSL.Forum.Web.Models.Home.IndexViewModel;

namespace OSL.Forum.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(cfg =>
                {
                    //Register Mapper Profile
                    cfg.AddProfile<WebProfiles>();
                    cfg.AddProfile<CoreProfiles>();
                }
            )).AsSelf().InstancePerRequest();

            builder.Register(c =>
                {
                    //This resolves a new context that can be used later.
                    var context = c.Resolve<IComponentContext>();
                    var config = context.Resolve<MapperConfiguration>();
                    return config.CreateMapper(context.Resolve);
                })
                .As<IMapper>()
                .InstancePerLifetimeScope();

            //Service Classes
            builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerLifetimeScope();

            builder.RegisterType<CreateCategoryModel>().AsSelf();
            builder.RegisterType<CategoriesModel>().AsSelf();
            builder.RegisterType<EditCategoryModel>().AsSelf();
            builder.RegisterType<CategoryDetailsModel>().AsSelf();
            builder.RegisterType<EditForumModel>().AsSelf();
            builder.RegisterType<CreateForumModel>().AsSelf();
            builder.RegisterType<IndexViewModel>().AsSelf();
            builder.RegisterType<DetailsModel>().AsSelf();
            builder.RegisterType<TopicViewModel>().AsSelf();
            builder.RegisterType<CreateTopicModel>().AsSelf();
            builder.RegisterType<TopicDetailsModel>().AsSelf();
        }
    }
}