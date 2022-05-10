using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;
using System.Linq;
using System.Web;

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
    }
}