using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Entities
{
    [Table("Categories")]
    public class Category : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public virtual IList<Forum> Forums { get; set; }
    }
}
