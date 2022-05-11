using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class PostModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Post Title")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText), AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        public long TopicId { get; set; }
        public BO.Topic BoTopic { get; set; }
        public DateTime Time { get; set; }
        public string ApplicationUserId { get; set; }
        public BO.Topic Topic { get; set; }

        public PostModel()
        {
            
        }

        public PostModel(BO.Post post)
        {
            FillUpModelData(post);
        }

        public void FillUpModelData(BO.Post post)
        {
            this.Id = post.Id;
            this.Name = post.Name;
            this.Description = post.Description;
            this.ApplicationUserId = post.ApplicationUserId;
            this.TopicId = post.TopicId;
            this.Topic = post.Topic;
        }

        public BO.Post PostBuilder()
        {
            var post = new BO.Post()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                ApplicationUserId = this.ApplicationUserId,
                TopicId = this.TopicId,
                ModificationDate = this.Time
            };

            return post;
        }
    }
}