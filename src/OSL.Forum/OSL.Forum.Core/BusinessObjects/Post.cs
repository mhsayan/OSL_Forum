using System;

namespace OSL.Forum.Core.BusinessObjects
{
    public class Post
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public string ApplicationUserId { get; set; }
        public string Status { get; set; }
        public bool Owner { get; set; }
        public string OwnerName { get; set; }
    }
}
