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
    public class FavoriteForumModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public Pager Pager { get; set; }
    }
}