using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OSL.Forum.Common.Utilities;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models
{
    public class CategoryModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public IList<string> Roles { get; set; }
        public IList<BO.Category> Categories { get; set; }
        public Pager Pager { get; set; }
        public BO.Category Category { get; set; }
        public IList<BO.Forum> Forums { get; set; }

        public BO.Category CategoryBuilder()
        {
            var category = new BO.Category
            {
                Id = this.Id,
                Name = this.Name
            };

            return category;
        }
    }
}