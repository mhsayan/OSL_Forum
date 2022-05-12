using System;
using System.Collections.Generic;

namespace OSL.Forum.Entities.BusinessObjects
{
    public class Forum
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<Topic> Topics { get; set; }
    }
}
