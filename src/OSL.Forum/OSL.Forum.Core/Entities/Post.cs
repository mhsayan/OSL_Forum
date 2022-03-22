using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;
using OSL.Forum.Core.Enums;

namespace OSL.Forum.Core.Entities
{
    public class Post : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Status Status { get; set; }
    }
}
