using System.Collections.Generic;
using OSL.Forum.Common.Utilities;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class FavoriteForumModel
    {
        public IList<BO.FavoriteForum> FavoriteForums { get; set; }
        public Pager Pager { get; set; }
    }
}