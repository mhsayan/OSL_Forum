using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class CreateTopicModel
    {
        [Required]
        [Display(Name = "Topic Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        public Guid ForumId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;
        private IMapper _mapper;

        public CreateTopicModel()
        {
        }

        public CreateTopicModel(ICategoryService categoryService,
            IMapper mapper, IDateTimeUtility dateTimeUtility,
            IProfileService profileService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _dateTimeUtility = dateTimeUtility;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _profileService = _scope.Resolve<IProfileService>();
        }

        public void Create()
        {
            var user = _profileService.GetUser();

            if (user == null)
                throw new InvalidOperationException(nameof(user));

            var time = _dateTimeUtility.Now;

            var topic = new BO.Topic()
            {
                Name = this.Name,
                CreationDate = time,
                ModificationDate = time,
                ApplicationUserId = user.Id,
                ForumId = this.ForumId,
                Status = Status.Pending.ToString()
            };
        }
    }
}