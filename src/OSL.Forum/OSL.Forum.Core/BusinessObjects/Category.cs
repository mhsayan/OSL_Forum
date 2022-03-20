using System;
using OSL.Forum.Base;

namespace OSL.Forum.Core.BusinessObjects
{
    public class Category : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
