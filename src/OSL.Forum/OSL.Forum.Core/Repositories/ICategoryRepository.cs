using System;
using System.Collections.Generic;
using OSL.Forum.Base;
using OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Repositories
{
    public interface ICategoryRepository : IDisposable
    {
        void Save();
        Category GetByName(string name);
        Category GetById(long categoryId);
        void RemoveById(long categoryId);
        void Add(Category category);
        long GetCount();
        IList<Category> Load(int pageIndex, int pageSize, bool tracking, string includedProperty = "");
    }
}