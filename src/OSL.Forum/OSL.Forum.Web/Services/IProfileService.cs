﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Web.Models;

namespace OSL.Forum.Web.Services
{
    public interface IProfileService
    {
        string UserID();
        ApplicationUser GetUser();
    }
}