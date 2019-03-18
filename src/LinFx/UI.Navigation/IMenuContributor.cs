using System.Threading.Tasks;

namespace LinFx.UI.Navigation
{
    public interface IMenuContributor
    {
        Task ConfigureMenuAsync(MenuConfigurationContext context);
    }
}
