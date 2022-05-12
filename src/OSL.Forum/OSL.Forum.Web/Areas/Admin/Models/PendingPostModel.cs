using System.Collections.Generic;
using OSL.Forum.Common.Utilities;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models
{
    public class PendingPostModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ApprovalStatus { get; set; }
        public long TopicId { get; set; }
        public BO.Post Post { get; set; }
        public List<BO.Post> Posts { get; set; }
        public Pager Pager { get; set; }
    }
}