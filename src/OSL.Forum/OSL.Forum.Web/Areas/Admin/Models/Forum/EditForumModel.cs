using System;
using System.ComponentModel.DataAnnotations;
using Autofac;
using AutoMapper;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Forum
{
    public class EditForumModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        private ILifetimeScope _scope;
        private IForumService _forumService;
        private ICategoryService _categoryService;
        private IDateTimeUtility _dateTimeUtility;
        private IMapper _mapper;

        public EditForumModel()
        {
        }

        public EditForumModel(ICategoryService categoryService,
            IMapper mapper, IForumService forumService,
            IDateTimeUtility dateTimeUtility)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _forumService = forumService;
            _dateTimeUtility = dateTimeUtility;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _forumService = _scope.Resolve<IForumService>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
        }

        public BO.Forum GetForum(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            var forum = _forumService.GetForum(forumId);

            if (forum == null)
                throw new InvalidOperationException("Forum not found");

            return forum;
        }

        public void Edit()
        {
            var modificationDate = _dateTimeUtility.Now;

            var forum = _mapper.Map<BO.Forum>(this);
            forum.ModificationDate = modificationDate;

            _forumService.EditForum(forum);
            _categoryService.UpdateModificationDate(modificationDate, forum.CategoryId);
        }
    }
}