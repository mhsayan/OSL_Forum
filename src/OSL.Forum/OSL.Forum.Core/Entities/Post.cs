using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OSL.Forum.Base;

namespace OSL.Forum.Core.Entities
{
    [Table("Posts")]
    public class Post : IEntity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public string ApplicationUserId { get; set; }
        public string Status { get; set; }
    }
}
