using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.UI.Navigation
{
    [Service]
    public class MenuManager : IMenuManager
    {
        private readonly NavigationOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public MenuManager(
            IOptions<NavigationOptions> options, 
            IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public async Task<ApplicationMenu> GetAsync(string name)
        {
            var menu = new ApplicationMenu(name);

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new MenuConfigurationContext(menu, scope.ServiceProvider);

                foreach (var contributor in _options.MenuContributors)
                {
                    await contributor.ConfigureMenuAsync(context);
                }
            }

            //NormalizeMenu(menu);

            return menu;
        }

        public async Task<ApplicationMenu[]> GetAllAsync()
        {
            var menus = new List<ApplicationMenu>();
            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var contributor in _options.MenuContributors)
                {
                    var menu = new ApplicationMenu();
                    var context = new MenuConfigurationContext(menu, scope.ServiceProvider);

                    await contributor.ConfigureMenuAsync(context);
                    menus.Add(menu);
                }
            }
            return menus.ToArray();
        }
    }
}
