using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OSL.Forum.Web.Areas.Admin.Controllers
{
    public class ForumController : Controller
    {
        // GET: Admin/Forum
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(Guid categoryId)
        {
            return View();
        }
    }
}