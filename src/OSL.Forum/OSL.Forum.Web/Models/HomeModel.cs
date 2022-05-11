using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Web.Services;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class HomeModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public IList<BO.Category> Categories { get; set; }
        public bool IsAuthenticated { get; set; }
        public BO.Category Category { get; set; }
        public IList<string> Roles { get; set; }
        public IList<BO.Forum> Forums { get; set; }
        public Pager Pager { get; set; }
    }
}