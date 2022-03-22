using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;

namespace OSL.Forum.Core.Entities
{
    [Table("Forums")]
    public class Forum : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string ApprovalType { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<Topic> Topics { get; set; }
    }
}
