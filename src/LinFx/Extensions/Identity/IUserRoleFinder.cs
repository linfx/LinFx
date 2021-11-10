using System.Threading.Tasks;

namespace LinFx.Extensions.Identity
{
    public interface IUserRoleFinder
    {
        Task<string[]> GetRolesAsync(string userId);
    }
}
