using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
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
        public long ForumId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public BO.Forum Forum { get; set; }
        public DateTime Time { get; set; }

        public BO.Topic TopicBuilder()
        {
            var topic = new BO.Topic()
            {
                Name = this.Name,
                CreationDate = this.Time,
                ModificationDate = this.Time,
                ApplicationUserId = ApplicationUser.Id,
                ForumId = this.ForumId,
                Status = Status.Pending.ToString(),
                ApprovalType = ApprovalType.Manual.ToString(),
                ActivityStatus = ActivityStatus.Active.ToString()
            };

            return topic;
        }

        public BO.Post PostBuilder(long topicId)
        {
            var post = new BO.Post()
            {
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.Time,
                ModificationDate = this.Time,
                ApplicationUserId = ApplicationUser.Id,
                TopicId = topicId,
                Status = Status.Pending.ToString()
            };

            return post;
        }
    }
}