using System;
using System.Collections.Generic;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.BusinessObjects
{
    public class Category : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public virtual IList<Forum> Forums { get; set; }
    }
}
