using LinFx.Application.Models;

namespace LinFx.Extensions.Identity
{
    public class UserInput : PagedAndSortedResultRequest
    {
        public string Filter { get; set; }
    }
}
