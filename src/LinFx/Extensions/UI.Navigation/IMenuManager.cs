using System.Threading.Tasks;

namespace LinFx.Extensions.UI.Navigation
{
    public interface IMenuManager
    {
        Task<ApplicationMenu> GetAsync(string name);

        Task<ApplicationMenu[]> GetAllAsync();
    }
}