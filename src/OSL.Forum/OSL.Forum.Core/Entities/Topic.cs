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
    [Table("Topics")]
    public class Topic : IEntity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long ForumId { get; set; }
        public virtual Forum Forum { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApprovalType { get; set; }
        public string Status { get; set; }
        public string ActivityStatus { get; set; }
        public virtual IList<Post> Posts { get; set; }
    }
}
