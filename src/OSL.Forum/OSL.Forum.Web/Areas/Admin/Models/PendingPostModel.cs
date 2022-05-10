using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BO = OSL.Forum.Core.BusinessObjects;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;

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