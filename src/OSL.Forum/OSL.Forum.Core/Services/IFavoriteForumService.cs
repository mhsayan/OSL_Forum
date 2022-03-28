using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
{
    public interface IFavoriteForumService
    {
        void CreateForum(BO.Forum forum);
        BO.Forum GetForum(string forumName, Guid categoryId);
        void EditForum(BO.Forum forum);
        BO.Forum GetForum(Guid forumId);
        void DeleteForum(Guid forumId);
    }
}
