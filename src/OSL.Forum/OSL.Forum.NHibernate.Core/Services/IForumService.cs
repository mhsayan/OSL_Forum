using System;
using System.Collections.Generic;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.NHibernate.Core.Services
{
    public interface IForumService
    {
        void CreateForum(BO.Forum forum);
        BO.Forum GetForum(string forumName, Guid categoryId);
        void EditForum(BO.Forum forum);
        BO.Forum GetForum(Guid forumId);
        void DeleteForum(Guid forumId);
        int GetForumCount(Guid categoryId);
        IList<BO.Forum> GetForums(int pagerCurrentPage, int pagerPageSize, Guid categoryId);
    }
}
