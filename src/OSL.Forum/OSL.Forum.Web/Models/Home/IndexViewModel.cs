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
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Home
{
    public class IndexViewModel
    {
        public IList<string> Roles { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private static readonly UserStore<ApplicationUser> UserStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
        private readonly ApplicationUserManager _userManager = new ApplicationUserManager(UserStore);

        public IndexViewModel()
        {
        }

        public IndexViewModel(ICategoryService categoryService,
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