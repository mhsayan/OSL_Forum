using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Entities;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoryDetailsModel : BaseModel
    {
        public BO.Category Category { get; set; }
        public IList<string> Roles { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private static readonly UserStore<ApplicationUser> UserStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
        private readonly ApplicationUserManager _userManager = new ApplicationUserManager(UserStore);
        public CategoryDetailsModel()
        {
        }

        public CategoryDetailsModel(ICategoryService categoryService,
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

        public void GetCategory(Guid categoryId)
        {
            Category = _categoryService.GetCategory(categoryId);
        }

        public async Task LoadUserInfo()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            Roles = await _userManager.GetRolesAsync(userId);
        }
    }
}