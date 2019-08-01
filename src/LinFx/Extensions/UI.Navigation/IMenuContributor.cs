using System.Threading.Tasks;

namespace LinFx.Extensions.UI.Navigation
{
    public interface IMenuContributor
    {
        Task ConfigureMenuAsync(MenuConfigurationContext context);
    }
}
