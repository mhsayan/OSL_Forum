using System;
using OSL.Forum.Base;

namespace OSL.Forum.Core.BusinessObjects
{
    public class Category : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Edit { get; set; }
    }
}
