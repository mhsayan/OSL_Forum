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
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoriesModel : BaseModel
    {
        public IList<string> Roles { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        public Pager Pager { get; set; }
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

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();

            await base.ResolveAsync(_scope);
        }

        public IList<BO.Category> GetCategories(int? page)
        {
            var totalItem = _categoryService.GetCategoryCount();

            var pager = new Pager(totalItem, page);

            return _categoryService.GetCategories();
        }

        public async Task LoadUserInfo()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            Roles = await _userManager.GetRolesAsync(userId);
        }

        public void Delete(Guid categoryId)
        {
            _categoryService.DeleteCategory(categoryId);
        }
    }
}