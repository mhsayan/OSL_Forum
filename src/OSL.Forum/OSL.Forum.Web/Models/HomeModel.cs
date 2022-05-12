using System.Collections.Generic;
using OSL.Forum.Common.Utilities;
using BO = OSL.Forum.Entities.BusinessObjects;

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