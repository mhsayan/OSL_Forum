using System;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.BusinessObjects
{
    public class FavoriteForum
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid ForumId { get; set; }
        public Entities.Forum Forum { get; set; }

    }
}
