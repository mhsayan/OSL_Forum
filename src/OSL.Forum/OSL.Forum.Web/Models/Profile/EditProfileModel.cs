using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Models.Profile
{
    public class EditProfileModel : BaseModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        private IProfileService _profileService;

        public EditProfileModel()
        {

        }

        public override async Task Resolve()
        {
            _profileService = new ProfileService();

            await base.Resolve();
        }

        public async Task EditProfileAsync()
        {
            var applicationUser = new ApplicationUser()
            {
                Id = Id,
                Name = Name
            };

            await _profileService.EditProfileAsync(applicationUser);
        }

        public void LoadUserInfo()
        {
            var user = _profileService.GetUser();

            this.Id = user.Id;
            this.Name = user.Name;
        }
    }
}