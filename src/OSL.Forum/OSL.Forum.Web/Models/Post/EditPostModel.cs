using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Post
{
    public class EditPostModel : BaseModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Post Title")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public long TopicId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Description")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        public BO.Topic Topic { get; set; }
        private DateTime Time { get; set; }
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;

        public EditPostModel()
        {
        }

        public override async Task Resolve()
        {
            _dateTimeUtility = DateTimeUtility.Create();
            _topicService = TopicService.Create();
            _postService = PostService.Create();

            await base.Resolve();
        }

        public void GetPost(long postId)
        {
            var post = _postService.GetPost(postId);

            this.Id = post.Id;
            this.Name = post.Name;
            this.Description = post.Description;
            this.ApplicationUserId = post.ApplicationUserId;
            this.TopicId = post.TopicId;
            this.Topic = post.Topic;
        }

        public void EditPost()
        {
            Time = _dateTimeUtility.Now;

            var post = new BO.Post()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                ApplicationUserId = this.ApplicationUserId,
                TopicId = this.TopicId,
                Topic = this.Topic,
                ModificationDate = Time,
                Status = Status.Pending.ToString()
            };

            _postService.EditPost(post);
        }

        public void UpdateTopicModificationDate()
        {
            _topicService.UpdateModificationDate(TopicId, Time);
        }

        public void Delete(long postId)
        {
            _postService.DeletePost(postId);
        }
    }
}