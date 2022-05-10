using System.Threading.Tasks;
using OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Web.Models;
using OSL.Forum.Web.Services;

namespace OSL.Forum.Web.Areas.Admin.Models.PendingPost
{
    public class PendingPostDetailsModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ApprovalStatus { get; set; }
        public long TopicId { get; set; }
        public Post Post { get; set; }
        private IPostService _postService;
        private IProfileService _profileService;
        private ITopicService _topicService;

        public PendingPostDetailsModel()
        {
            _postService = new PostService();
            _profileService = new ProfileService();
            _topicService = new TopicService();
        }

        public void GetPendingPost(long postId)
        {
            Post = _postService.GetPost(postId);
            Post.OwnerName = _profileService.GetUser(Post.ApplicationUserId).Name;
            this.Id = Post.Id;
            this.Name = Post.Name;
            this.Description = Post.Description;
            this.TopicId = Post.TopicId;
        }

        public void UpdatePostStatus(string status)
        {
            _postService.UpdatePostStatus(Id, status);
        }

        public void UpdateTopicApprovalType()
        {
            if (ApprovalStatus)
                _topicService.UpdateTopicApprovalType(TopicId);
        }
    }
}