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
    public class Topic : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid ForumId { get; set; }
        public virtual Forum Forum { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Status Status { get; set; }
        public virtual IList<Post> Posts { get; set; }
    }
}
