using System.Threading.Tasks;

namespace OSL.Forum.Web.Models
{
    public class ConfirmEmailModel : BaseModel
    {
        public bool Status { get; set; }

        public ConfirmEmailModel()
        {

        }

        public override async Task Resolve()
        {
            await base.Resolve();
        }
    }
}