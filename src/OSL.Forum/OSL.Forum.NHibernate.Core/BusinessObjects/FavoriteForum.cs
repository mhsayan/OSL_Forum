using System;
using System.ComponentModel.DataAnnotations.Schema;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.BusinessObjects
{
    public class FavoriteForum
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public Guid ForumId { get; set; }
        public Forum Forum { get; set; }

    }
}
