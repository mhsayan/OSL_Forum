using System;
using System.ComponentModel.DataAnnotations;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Forum
{
    public class CreateForumModel
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private ILifetimeScope _scope;
        private IProfileService _profileService;
        private IDateTimeUtility _dateTimeUtility;

        public CreateForumModel()
        {
        }

        public CreateForumModel(ICategoryService categoryService,
            IProfileService profileService,
            IDateTimeUtility dateTimeUtility)
        {
            _profileService = profileService;
            _dateTimeUtility = dateTimeUtility;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
        }

        public void Create()
        {
            var user = _profileService.GetUser();

            var forum = new BO.Forum()
            {
                Name = this.Name,
                CategoryId = this.CategoryId,
                ApplicationUserId = user.Id,
                CreationDate = _dateTimeUtility.Now,
                ModificationDate = _dateTimeUtility.Now,
                ApprovalType = ApprovalType.Manual.ToString()
            };


        }
    }
}