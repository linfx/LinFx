using LinFx.Application.Contracts;

namespace LinFx.Extensions.Identity
{
    public class UserInput : PagedAndSortedResultRequest
    {
        public string Filter { get; set; }
    }
}
