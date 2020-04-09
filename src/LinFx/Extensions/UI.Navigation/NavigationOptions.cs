using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.UI.Navigation
{
    public class NavigationOptions
    {
        [NotNull]
        public List<IMenuContributor> MenuContributors { get; }

        public NavigationOptions()
        {
            MenuContributors = new List<IMenuContributor>();
        }
    }
}