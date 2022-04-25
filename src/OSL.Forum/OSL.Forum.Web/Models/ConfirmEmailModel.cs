using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;

namespace OSL.Forum.Web.Models
{
    public class ConfirmEmailModel : BaseModel
    {
        public bool Status { get; set; }

        public ConfirmEmailModel()
        {

        }

        public override Task Resolve()
        {
            return Task.CompletedTask;
        }
    }
}