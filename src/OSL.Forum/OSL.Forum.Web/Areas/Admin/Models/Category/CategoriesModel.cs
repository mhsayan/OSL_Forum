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
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoriesModel
    {
        public IList<string> Roles { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private static readonly UserStore<ApplicationUser> UserStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
        private readonly ApplicationUserManager _userManager = new ApplicationUserManager(UserStore);
        public CategoriesModel()
        {
        }

        public CategoriesModel(ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public IList<BO.Category> GetCategories()
        {
            return _categoryService.GetCategories();
        }

        public async Task LoadUserInfo()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            Roles = await _userManager.GetRolesAsync(userId);
        }
    }
}