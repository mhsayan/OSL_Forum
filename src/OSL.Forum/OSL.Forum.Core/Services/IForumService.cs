using System.Collections.Generic;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
{
    public interface IForumService
    {
        void CreateForum(BO.Forum forum);
        BO.Forum GetForum(string forumName, long categoryId);
        void EditForum(BO.Forum forum);
        BO.Forum GetForum(long forumId);
        void DeleteForum(long forumId);
        long GetForumCount(long categoryId);
        IList<BO.Forum> GetForums(int pagerCurrentPage, int pagerPageSize, long categoryId);
    }
}
