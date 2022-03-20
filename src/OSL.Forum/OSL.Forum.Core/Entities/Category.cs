using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;

namespace OSL.Forum.Core.Entities
{
    public class Category : IEntity<Guid>
    {
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EditTime { get; set; }
    }
}
