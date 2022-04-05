using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using OSL.Forum.Core.Services;
using BO = OSL.Forum.Core.BusinessObjects;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Entities;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Membership.Entities;
using OSL.Forum.Membership.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoriesModel : BaseModel
    {
        public IList<string> Roles { get; set; }
        public IList<BO.Category> Categories { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        public Pager Pager { get; set; }
        private IProfileService _profileService;

        public CategoriesModel()
        {
        }

        public CategoriesModel(ICategoryService categoryService,
            IMapper mapper, IProfileService profileService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _profileService = profileService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _profileService = _scope.Resolve<IProfileService>();

            await base.ResolveAsync(_scope);
        }

        public void GetCategories(int? page)
        {
            var totalItem = _categoryService.GetCategoryCount();

            Pager = new Pager(totalItem, page);

            Categories = _categoryService.GetCategories(Pager.CurrentPage, Pager.PageSize);
        }

        public async Task LoadUserInfo()
        {
            Roles = await _profileService.UserRolesAsync();
        }

        public void Delete(Guid categoryId)
        {
            _categoryService.DeleteCategory(categoryId);
        }
    }
}