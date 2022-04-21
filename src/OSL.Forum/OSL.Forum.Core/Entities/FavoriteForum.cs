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
    [Table("FavoriteForums")]
    public class FavoriteForum : IEntity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("Forum")]
        public long ForumId { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
