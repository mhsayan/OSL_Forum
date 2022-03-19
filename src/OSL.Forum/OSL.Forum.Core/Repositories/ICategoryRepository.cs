using System;
using OSL.Forum.Base;
using OSL.Forum.Core.Contexts;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {

    }
}