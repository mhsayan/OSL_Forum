﻿using System;
using OSL.Forum.NHibernate.Core.Entities;
using OSL.Forum.NHibernateBase;

namespace OSL.Forum.NHibernate.Core.Repositories
{
    public interface IPostRepository : IRepository<Post, Guid>
    {

    }
}