using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OSL.Forum.Base;

namespace OSL.Forum.Core.BusinessObjects
{
    public class Forum : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApprovalType { get; set; }
        public virtual Entities.Category Category { get; set; }
        public virtual IList<Topic> Topics { get; set; }
    }
}
