using System.ComponentModel.DataAnnotations;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models
{
    public class ForumModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public BO.Forum BoForum { get; set; }
        [Required]
        public long CategoryId { get; set; }
        public BO.Category BoCategory { get; set; }
    }
}