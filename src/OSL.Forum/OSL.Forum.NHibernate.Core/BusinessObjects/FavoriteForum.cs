using System;
using OSL.Forum.Membership.Entities;

namespace OSL.Forum.NHibernate.Core.BusinessObjects
{
    public class FavoriteForum
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public Guid ForumId { get; set; }
        public Forum Forum { get; set; }

    }
}
