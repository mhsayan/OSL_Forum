using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OSL.Forum.Base;

namespace OSL.Forum.Core.BusinessObjects
{
    public class Topic : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid ForumId { get; set; }
        public virtual Entities.Forum Forum { get; set; }
        public string ApplicationUserId { get; set; }
        public string Status { get; set; }
        public virtual IList<Post> Posts { get; set; }
    }
}
