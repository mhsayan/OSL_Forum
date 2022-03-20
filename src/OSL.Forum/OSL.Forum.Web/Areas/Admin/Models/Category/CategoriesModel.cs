using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoriesModel
    {
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IMapper _mapper;

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
    }
}