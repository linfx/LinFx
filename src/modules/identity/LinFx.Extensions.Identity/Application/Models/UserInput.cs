using LinFx.Application.Models;

namespace LinFx.Extensions.Identity.Application.Models
{
    public class UserInput : PagedAndSortedResultRequest
    {
        public string Filter { get; set; }
    }
}
