using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSL.Forum.Entities
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
