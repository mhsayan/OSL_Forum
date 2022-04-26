using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OSL.Forum.Base;

namespace OSL.Forum.Core.Entities
{
    [Table("Forums")]
    public class Forum : IEntity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<Topic> Topics { get; set; }
    }
}
