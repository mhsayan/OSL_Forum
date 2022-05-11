using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class TopicModel
    {
        public BO.Forum Forum { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsFavorite { get; set; }
        public IList<BO.Topic> Topics { get; set; }
        public IList<string> UserRoles { get; set; }
        public Pager Pager { get; set; }
        private ApplicationUser ApplicationUser { get; set; }
        public BO.Topic Topic { get; set; }
        public DateTime Time { get; set; }
    }
}