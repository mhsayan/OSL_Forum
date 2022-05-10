using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models.Post
{
    public class CreatePostModel : BaseModel
    {
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
        private DateTime Time { get; set; }
        private ITopicService _topicService;
        private IPostService _postService;
        private IDateTimeUtility _dateTimeUtility;
        private IProfileService _profileService;

        public CreatePostModel()
        {
        }

        public override async Task Resolve()
        {
            _dateTimeUtility = new DateTimeUtility();
            _profileService = new ProfileService();
            _topicService = new TopicService();
            _postService = new PostService();

            await base.Resolve();
        }

        public void CreatePost()
        {
            if (BoTopic.ActivityStatus != ActivityStatus.Active.ToString())
                return;

            var user = _profileService.GetUser();
            Time = _dateTimeUtility.Now;

            var post = new BO.Post()
            {
                Name = this.Name,
                Description = this.Description,
                TopicId = this.TopicId,
                CreationDate = Time,
                ModificationDate = Time,
                ApplicationUserId = user.Id
            };

            if (BoTopic.ApprovalType == ApprovalType.Auto.ToString())
                post.Status = Status.Approved.ToString();
            else post.Status = Status.Pending.ToString();

            _postService.CreatePost(post);
        }

        public void UpdateTopicModificationDate()
        {
            _topicService.UpdateModificationDate(TopicId, Time);
        }

        public void GetTopic(long topicId)
        {
            BoTopic = _topicService.GetTopic(topicId);
        }
    }
}