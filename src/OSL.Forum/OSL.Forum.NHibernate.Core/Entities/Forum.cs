﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Membership.Entities;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Entities
{
    [Table("Forums")]
    public class Forum : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }
        [Required]
        [MaxLength(64)]
        public virtual string Name { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ModificationDate { get; set; }
        public virtual Guid CategoryId { get; set; }
        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<Topic> Topics { get; set; }
    }
}