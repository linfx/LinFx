using LinFx.Application.Models;

namespace LinFx.Module.Identity.ViewModels
{
    public class UserInput : PagedAndSortedResultRequest
    {
        public string Filter { get; set; }
    }
}
