using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.UI.Navigation
{
    public class ApplicationMenu
    {
        /// <summary>
        /// Unique name of the menu in the application.
        /// </summary>
        [NotNull]
        public string Name { get; set; }

        /// <summary>
        /// Display name of the menu.
        /// Default value is the <see cref="Name"/>.
        /// </summary>
        [NotNull]
        public string DisplayName { get; set; }

        [NotNull]
        public IList<ApplicationMenuItem> Items { get; } = new List<ApplicationMenuItem>();

        public ApplicationMenu() { }

        public ApplicationMenu([NotNull] string name, string displayName = default)
        {
            Name = name;
            DisplayName = displayName;
        }

        /// <summary>
        /// Adds a <see cref="ApplicationMenuItem"/> to <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem"><see cref="ApplicationMenuItem"/> to be added</param>
        /// <returns>This <see cref="ApplicationMenu"/> object</returns>
        public ApplicationMenu AddItem([NotNull] ApplicationMenuItem menuItem)
        {
            Items.Add(menuItem);
            return this;
        }
    }
}
