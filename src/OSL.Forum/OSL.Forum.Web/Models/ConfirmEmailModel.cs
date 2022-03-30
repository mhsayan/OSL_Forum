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
        private ILifetimeScope _scope;

        public ConfirmEmailModel()
        {

        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;

            await base.ResolveAsync(_scope);
        }
    }
}