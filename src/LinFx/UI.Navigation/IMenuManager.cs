using System.Threading.Tasks;

namespace LinFx.UI.Navigation
{
    public interface IMenuManager
    {
        Task<ApplicationMenu> GetAsync(string name);

        Task<ApplicationMenu[]> GetAllAsync();
    }
}