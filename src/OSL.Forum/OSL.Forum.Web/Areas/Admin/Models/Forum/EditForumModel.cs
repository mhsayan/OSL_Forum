using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.NHibernate.Core.Utilities;
using OSL.Forum.Membership.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Forum
{
    public class EditForumModel : BaseModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public BO.Forum BoForum { get; set; }
        private ILifetimeScope _scope;
        private IForumService _forumService;
        private ICategoryService _categoryService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;
        private IMapper _mapper;

        public EditForumModel()
        {
        }

        public EditForumModel(ICategoryService categoryService,
            IMapper mapper, IForumService forumService,
            IDateTimeUtility dateTimeUtility,
            IProfileService profileService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _forumService = forumService;
            _dateTimeUtility = dateTimeUtility;
            _profileService = profileService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _forumService = _scope.Resolve<IForumService>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _profileService = _scope.Resolve<IProfileService>();

            await base.ResolveAsync(_scope);
        }

        public void GetForum(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            BoForum = _forumService.GetForum(forumId);

            if (BoForum == null)
                throw new InvalidOperationException("Forum not found");

            _mapper.Map(BoForum, this);
        }

        public void Edit()
        {
            var user = _profileService.GetUser();
            var modificationDate = _dateTimeUtility.Now;

            var forum = _mapper.Map<BO.Forum>(this);
            forum.ModificationDate = modificationDate;
            forum.ApplicationUserId = user.Id;

            _forumService.EditForum(forum);
            _categoryService.UpdateModificationDate(modificationDate, forum.CategoryId);
        }

        public void Delete(Guid forumId)
        {
            if (forumId == Guid.Empty)
                throw new ArgumentNullException(nameof(forumId));

            _forumService.DeleteForum(forumId);
        }
    }
}