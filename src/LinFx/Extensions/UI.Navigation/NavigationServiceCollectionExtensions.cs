using LinFx.Extensions.UI.Navigation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NavigationServiceCollectionExtensions
    {
        public static LinFxBuilder AddNavigation(this LinFxBuilder builder, Action<NavigationOptions> optionsAction)
        {
            builder.Services.Configure(optionsAction);
            builder.Services.AddSingleton<IMenuManager, MenuManager>();
            return builder;
        }
    }
}
