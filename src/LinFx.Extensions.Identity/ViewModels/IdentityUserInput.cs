using LinFx.Application.Models;

namespace LinFx.Module.Identity.ViewModels
{
    public class IdentityUserInput : PagedAndSortedResultRequest
    {
        public string Filter { get; set; }
    }
}
