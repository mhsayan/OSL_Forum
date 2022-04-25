﻿using System;
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
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoriesModel : BaseModel
    {
        public IList<string> Roles { get; set; }
        public IList<BO.Category> Categories { get; set; }
        public Pager Pager { get; set; }
        private ICategoryService _categoryService;
        private IProfileService _profileService;

        public CategoriesModel()
        {
        }

        public override Task Resolve()
        {
            _categoryService = CategoryService.Create();
            _profileService = ProfileService.Create();

            return Task.CompletedTask;
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

        public void Delete(long categoryId)
        {
            _categoryService.DeleteCategory(categoryId);
        }
    }
}