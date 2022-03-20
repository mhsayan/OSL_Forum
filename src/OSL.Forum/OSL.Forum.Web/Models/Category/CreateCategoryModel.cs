using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Category
{
    public class CreateCategoryModel
    {
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private ILifetimeScope _scope;
        protected ICategoryService _categoryService;
        private IMapper _mapper;

        public CreateCategoryModel()
        {
            //_categoryService = DependencyResolver.Current.GetService<ICategoryService>();
            //_mapper = DependencyResolver.Current.GetService<IMapper>();
        }

        public CreateCategoryModel(ICategoryService categoryService,
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

        public void Create()
        {
            var category = _mapper.Map<BO.Category>(this);

            _categoryService.CreateCategory(category);
        }
    }
}