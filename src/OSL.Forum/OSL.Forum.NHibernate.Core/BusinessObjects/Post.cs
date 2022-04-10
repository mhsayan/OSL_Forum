﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OSL.Forum.Membership.Entities;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.BusinessObjects
{
    public class Post : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Status { get; set; }
        public bool Owner { get; set; }
        public string OwnerName { get; set; }
    }
}